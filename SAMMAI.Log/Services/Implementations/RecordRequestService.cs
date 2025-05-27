using Microsoft.Extensions.Options;
using SAMMAI.Authentication.Utility.SettingsFiles;
using SAMMAI.Log.Repository;
using SAMMAI.Log.Services.Interfaces;
using SAMMAI.Log.Utility.Constants;
using SAMMAI.Transverse.Helpers;
using SAMMAI.Transverse.Models.Endpoints.Log.RecordRequest;
using SAMMAI.Transverse.Models.Objects;
using System.Text;
using MSDataBase = SAMMAI.Transverse.Models.Endpoints.DataBase;

namespace SAMMAI.Log.Services.Implementations
{
    public class RecordRequestService : IRecordRequestService
    {
        private readonly ILogger<RecordRequestService> _logger;
        private readonly ProjectSettings _projectSettings;
        private readonly RecordRequestRepository _recordRequestRepository;

        public RecordRequestService(
            ILogger<RecordRequestService> logger,
            IOptions<ProjectSettings> projectSettingsOptions,
            RecordRequestRepository recordRequestRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _projectSettings = projectSettingsOptions?.Value ?? throw new ArgumentNullException(nameof(projectSettingsOptions));
            _recordRequestRepository = recordRequestRepository ?? throw new ArgumentNullException(nameof(recordRequestRepository));
        }

        public async Task<InsertRequestResponse> InsertRequest(InsertRequestRequest input)
        {
            InsertRequestResponse output;
            SegRecordRequestObject recordRequest;
            Uri? uri;
            string modulo;
            string urlRequest;
            string pathFile;
            string fileName;
            string pathFolder;
            string recordRequestCodigo;

            recordRequestCodigo = Guid.NewGuid().ToString("N");
            urlRequest = $"[{input.IpAddress}] [{input.UrlRequest}]";
            uri = new Uri(input.UrlRequest);
            modulo = string.Concat(uri?.Segments.GetValue(2), uri?.Segments.GetValue(3));

            fileName = string.Format(GeneralConstants.FormatFileName.RequestLog, recordRequestCodigo);
            pathFolder = Path.Combine(Directory.GetCurrentDirectory(), _projectSettings.RecordRequestLogPathFolder, input.Application);
            pathFile = input.Body.WriteToFile(pathFolder, fileName, true);

            recordRequest = new SegRecordRequestObject()
            {
                Application = input.Application,
                RecordRequestCode = recordRequestCodigo,
                Module = modulo,
                UrlService = urlRequest,
                SizeRequest = input.ContentLength ?? Encoding.ASCII.GetByteCount(input.Body),
                RecordRequest = pathFile,
                IdUser = input.IdUser
            };

            await _recordRequestRepository.Insert(recordRequest);

            output = new InsertRequestResponse()
            {
                RecordRequestCode = recordRequestCodigo
            };

            return output;
        }

        public async Task InsertResponse(InsertResponseRequest input)
        {
            MSDataBase.RecordRequest.UpdateResponseRequest updateRecordRequest;
            string pathFile;
            string fileName;
            string pathFolder;

            fileName = string.Format(GeneralConstants.FormatFileName.ResponseLog, input.RecordRequestCode);
            pathFolder = Path.Combine(_projectSettings.RecordRequestLogPathFolder, input.Application);
            pathFile = input.Body.WriteToFile(pathFolder, fileName, true);

            updateRecordRequest = new MSDataBase.RecordRequest.UpdateResponseRequest()
            {
                RecordRequestCode = input.RecordRequestCode,
                IsSuccess = IsSuccessStatusCode((StatusCodeEnum)input.StatusCode),
                ResponseMessage = pathFile
            };

            await _recordRequestRepository.UpdateResponse(updateRecordRequest);
        }

        private static bool IsSuccessStatusCode(StatusCodeEnum statusCode)
            => statusCode switch
            {
                StatusCodeEnum.OK or StatusCodeEnum.CREATED or StatusCodeEnum.NO_CONTENT => true,
                _ => false,
            };
    }
}
