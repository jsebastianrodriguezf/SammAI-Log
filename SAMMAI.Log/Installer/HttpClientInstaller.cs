using SAMMAI.Transverse.ExternalServices;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using static SAMMAI.Transverse.Constants.GeneralConstants;

namespace SAMMAI.Log.Installer
{
    public class HttpClientInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<DataBaseHttpService>(HttpClientSetting.API_Client_DataBase, client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection("ProjectSettings:SAMMAIMicroservices:DataBase:BaseRoute")?.Value ?? string.Empty);
            }).
            ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                SslProtocols = SslProtocols.Tls12,
                ClientCertificateOptions = ClientCertificateOption.Automatic,
                PreAuthenticate = true,
                ServerCertificateCustomValidationCallback = UseInsecureChannel(configuration, "ProjectSettings:SAMMAIMicroservices:DataBase:UseInsecureChannel")
            });

            services.AddHttpClient<AuthenticationHttpService>(HttpClientSetting.API_Client_Authentication, client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection("ProjectSettings:SAMMAIMicroservices:Authentication:BaseRoute")?.Value ?? string.Empty);
            }).
            ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                SslProtocols = SslProtocols.Tls12,
                ClientCertificateOptions = ClientCertificateOption.Automatic,
                PreAuthenticate = true,
                ServerCertificateCustomValidationCallback = UseInsecureChannel(configuration, "ProjectSettings:SAMMAIMicroservices:Authentication:UseInsecureChannel")
            });
        }

        private static Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>? UseInsecureChannel(IConfiguration configuration, string value)
        {
            if (configuration.GetValue<bool?>(value) != true)
                return null;

            return (message, cert, chain, errors) => true;
        }
    }
}
