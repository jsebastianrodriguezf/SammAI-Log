using Microsoft.Extensions.Options;
using SAMMAI.Authentication.Utility.SettingsFiles;
using SAMMAI.Transverse.Abstracts;
using SAMMAI.Transverse.ExternalServices;
using SAMMAI.Transverse.Helpers;
using SAMMAI.Transverse.Models.Objects;
using static SAMMAI.Transverse.Constants.ApiRoutes.DataBaseAPI;

namespace SAMMAI.Log.Repository
{
    public class LogTraceabilityRepository : BaseDataBase
    {
        private readonly ILogger<LogTraceabilityRepository> _logger;
        private readonly DataBaseHttpService _dataBaseHttpService;
        private readonly Global _global;

        public LogTraceabilityRepository(
            ILogger<LogTraceabilityRepository> logger,
            IOptions<ProjectSettings> projectSettingsOptions,
            DataBaseHttpService dataBaseHttpService,
            Global global)
            : base(
                    dataBaseHttpService,
                    global?.GetAuthorizationHeaders()!)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataBaseHttpService = dataBaseHttpService ?? throw new ArgumentNullException(nameof(dataBaseHttpService));
            _global = global ?? throw new ArgumentNullException(nameof(global));
        }

        public async Task<GenLogTraceabilityObject> Insert(GenLogTraceabilityObject input)
        {
            return await Insert<GenLogTraceabilityObject>(BaseController.LogTraceability, input);
        }
    }
}