using Microsoft.OpenApi.Models;
using SAMMAI.Log.Utility.Constants;
using System.Reflection;
using static SAMMAI.Transverse.Constants.GeneralConstants;

namespace SAMMAI.Log.Installer
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(SwaggerConstants.Version, new OpenApiInfo
                {
                    Title = string.Format(SwaggerConstants.Title, SwaggerConstants.Version),
                    Version = SwaggerConstants.Version,
                    Description = SwaggerConstants.Descripcion,
                    Contact = new OpenApiContact
                    {
                        Name = SwaggerConstants.NameContact,
                        Email = SwaggerConstants.EmailContact,
                        Url = new Uri(SwaggerConstants.UrlContact),
                    }
                });

                options.AddSecurityDefinition(AuthenticationHeader.Authorization, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = $"Please enter your token here (without '{SchemaToken.Bearer} ' prefix)",
                    Name = AuthenticationHeader.Authorization,
                    Type = SecuritySchemeType.Http,
                    Scheme = SchemaToken.Bearer,
                    BearerFormat = "JWT"
                });

                OpenApiSecurityScheme AuthorizationKey = new()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = AuthenticationHeader.Authorization
                    },
                    In = ParameterLocation.Header
                };

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        { AuthorizationKey, Array.Empty<string>() },
                    });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }
    }
}
