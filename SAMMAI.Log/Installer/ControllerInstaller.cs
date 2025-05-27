using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SAMMAI.Authentication.Utility.SettingsFiles;
using SAMMAI.Log.Repository;
using SAMMAI.Log.Services.Implementations;
using SAMMAI.Log.Services.Interfaces;
using SAMMAI.Log.Utility.ActionFilters;
using SAMMAI.Transverse.Helpers;

namespace SAMMAI.Log.Installer
{
    public class ControllerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ProjectSettings>(configuration.GetSection("ProjectSettings"));

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(FluentValidationFilter));
            });

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                //Allow to handle a bad response filter action
                options.SuppressModelStateInvalidFilter = true;
            });

            //Authentication
            services.AddTransient<AuthenticationHelper>();

            //Services
            services.AddScoped<IRecordRequestService, RecordRequestService>();
            services.AddScoped<ISerilogService, SerilogService>();
            services.AddScoped<ILogTraceabilityService, LogTraceabilityService>();

            //Repositories
            services.AddTransient<RecordRequestRepository>();
            services.AddTransient<LogTraceabilityRepository>();

            services.AddHttpContextAccessor();
            services.AddScoped<Global>();
        }
    }
}
