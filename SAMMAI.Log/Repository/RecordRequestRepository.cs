using Microsoft.Extensions.Options;
using SAMMAI.Authentication.Utility.SettingsFiles;
using SAMMAI.Transverse.ExternalServices;
using SAMMAI.Transverse.Helpers;
using SAMMAI.Transverse.Models.Endpoints.DataBase.RecordRequest;
using SAMMAI.Transverse.Models.Objects;
using SAMMAI.Transverse.Models.Response.BaseApi;

namespace SAMMAI.Log.Repository
{
    public class RecordRequestRepository
    {
        private readonly ILogger<RecordRequestRepository> _logger;
        private readonly SAMMAIMicroservices _sammaiMicroservicesOptions;
        private readonly DataBaseHttpService _dataBaseHttpService;
        private readonly Global _global;

        public RecordRequestRepository(
            ILogger<RecordRequestRepository> logger,
            IOptions<ProjectSettings> projectSettingsOptions,
            DataBaseHttpService dataBaseHttpService,
            Global global)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sammaiMicroservicesOptions = projectSettingsOptions?.Value.SAMMAIMicroservices ?? throw new ArgumentNullException(nameof(projectSettingsOptions));
            _dataBaseHttpService = dataBaseHttpService ?? throw new ArgumentNullException(nameof(dataBaseHttpService));
            _global = global ?? throw new ArgumentNullException(nameof(global));
        }

        public async Task<SegRecordRequestObject> Insert(SegRecordRequestObject input)
        {
            string url;
            List<StatusCodeEnum> statusCodesExpected;
            BaseApiResponseWithData<SegRecordRequestObject> response;

            url = _sammaiMicroservicesOptions.DataBase.InsertRecordRequestRequest;
            statusCodesExpected = [StatusCodeEnum.CREATED];
            response = await _dataBaseHttpService.RestAsync<BaseApiResponseWithData<SegRecordRequestObject>>(url, HttpMethod.Post, statusCodesExpected, input, _global.GetAuthorizationHeaders());

            return (StatusCodeEnum)response.StatusCode switch
            {
                StatusCodeEnum.CREATED => response.Data,
                _ => throw new ApiException((StatusCodeEnum)response.StatusCode, response.Message),
            };
        }

        public async Task<bool> UpdateResponse(UpdateResponseRequest input)
        {
            string url;
            List<StatusCodeEnum> statusCodesExpected;
            BaseApiResponse response;

            url = _sammaiMicroservicesOptions.DataBase.UpdateRecordRequestResponse;
            statusCodesExpected = [];
            response = await _dataBaseHttpService.RestAsync<BaseApiResponse>(url, HttpMethod.Patch, statusCodesExpected, input, _global.GetAuthorizationHeaders());

            return (StatusCodeEnum)response.StatusCode switch
            {
                StatusCodeEnum.NO_CONTENT => true,
                _ => throw new ApiException((StatusCodeEnum)response.StatusCode, response.Message),
            };
        }
    }
}
