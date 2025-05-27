using Microsoft.Extensions.Options;
using SAMMAI.Authentication.Utility.SettingsFiles;
using SAMMAI.Log.Repository;
using SAMMAI.Log.Services.Interfaces;
using SAMMAI.Log.Utility.Constants;
using SAMMAI.Transverse.Helpers;
using SAMMAI.Transverse.Models.Endpoints.Log.LogTraceability;
using SAMMAI.Transverse.Models.Objects;

namespace SAMMAI.Log.Services.Implementations
{
    public class LogTraceabilityService : ILogTraceabilityService
    {
        private readonly ILogger<LogTraceabilityService> _logger;
        private readonly ProjectSettings _projectSettings;
        private readonly LogTraceabilityRepository _logTraceabilityRepository;

        public LogTraceabilityService(
            ILogger<LogTraceabilityService> logger,
            IOptions<ProjectSettings> projectSettingsOptions,
            LogTraceabilityRepository logTraceabilityRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _projectSettings = projectSettingsOptions?.Value ?? throw new ArgumentNullException(nameof(projectSettingsOptions));
            _logTraceabilityRepository = logTraceabilityRepository ?? throw new ArgumentNullException(nameof(logTraceabilityRepository));
        }

        public async Task InsertLogTraceability(InsertLogTraceabilityRequest input)
        {
            GenLogTraceabilityObject logTraceability;
            string pathFile;
            string fileName;
            string pathFolder;
            string logTraceabilityCode;

            logTraceabilityCode = Guid.NewGuid().ToString("N");
            fileName = string.Format(GeneralConstants.FormatFileName.LogTraceability, logTraceabilityCode);
            pathFolder = Path.Combine(Directory.GetCurrentDirectory(), _projectSettings.LogTraceabiltyPathFolder, input.Application);
            pathFile = input.LogTraceability.WriteToFile(pathFolder, fileName, true);

            logTraceability = new GenLogTraceabilityObject()
            {
                LogTraceability = pathFile,
                LogTraceabilityCode = logTraceabilityCode,
                Application = input.Application,
                Version = input.Version,
                Level = input.Level,
                DateLog = input.DateLog,
                Cmm = input.Cmm
            };

            await _logTraceabilityRepository.Insert(logTraceability);
        }
    }
}
