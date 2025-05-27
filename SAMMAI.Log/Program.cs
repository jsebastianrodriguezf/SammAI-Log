global using SAMMAI.Transverse.Exceptions;
global using static SAMMAI.Transverse.Models.Response.BaseApi.ResponseCode;
using Microsoft.Extensions.Options;
using SAMMAI.Authentication.Utility.SettingsFiles;
using SAMMAI.Log.Installer;
using SAMMAI.Log.Utility.Constants;
using SAMMAI.Log.Utility.Extensions;
using Serilog;

namespace SAMMAI.Log
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.AddSerilog(new LoggerConfiguration().
                    ReadFrom.
                    Configuration(builder.Configuration).
                    CreateLogger());

            // Add services to the container.
            IServiceCollection services = builder.Services;
            services.InstallServiceAssembly(builder.Configuration);

            var app = builder.Build();

            app.Logger.LogInformation("Current environment: {environment}", app.Environment.EnvironmentName);

            // Configure the HTTP request pipeline.
            ProjectSettings projectSettings = app.Services.GetRequiredService<IOptions<ProjectSettings>>().Value;

            if (projectSettings.EnableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(string.Format(SwaggerConstants.UrlJsonSwagger, projectSettings.PathBase, SwaggerConstants.Version), string.Format(SwaggerConstants.Title, SwaggerConstants.Version));
                });
            }

            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
