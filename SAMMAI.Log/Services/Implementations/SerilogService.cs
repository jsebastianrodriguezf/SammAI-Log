using Microsoft.Extensions.Options;
using SAMMAI.Authentication.Utility.SettingsFiles;
using SAMMAI.Log.Models.Request;
using SAMMAI.Log.Services.Interfaces;
using SAMMAI.Log.Utility.Constants;
using SAMMAI.Transverse.Helpers;

namespace SAMMAI.Log.Services.Implementations
{
    public class SerilogService : ISerilogService
    {
        private readonly ILogger<SerilogService> _logger;
        private readonly ProjectSettings _projectSettings;

        public SerilogService(
            ILogger<SerilogService> logger,
            IOptions<ProjectSettings> projectSettingsOptions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _projectSettings = projectSettingsOptions?.Value ?? throw new ArgumentNullException(nameof(projectSettingsOptions)); ;
        }

        public async Task Insert(List<SerilogRequest> input)
        {
            string fileName;
            string pathFolder;

            foreach (SerilogRequest log in input)
            {
                try
                {
                    fileName = string.Format(GeneralConstants.FormatFileName.Serilog, log.Properties?.Application, DateTime.Now.ToString("ddMMyyyy"));
                    pathFolder = Path.Combine(Directory.GetCurrentDirectory(), _projectSettings.SerilogLogPathFolder);
                    input.ToJson().WriteToFile(pathFolder, fileName, true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error inserting log: {class} | {method}", nameof(SerilogService), nameof(Insert));
                    continue;
                }
            }
        }
    }
}
