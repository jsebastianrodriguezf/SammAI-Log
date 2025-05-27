using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using SAMMAI.Transverse.Helpers;
using SAMMAI.Transverse.Models.Authentication;
using static SAMMAI.Transverse.Constants.GeneralConstants;

namespace SAMMAI.Log.Utility.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PrivateIdentifierFilterAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            HttpContext httpContext = context.HttpContext;
            AuthPrivateDI authDI;

            IHeaderDictionary headers;
            Task<ValidateBearerTokenIntegrity.Output?> outputAuthTask;
            ValidateBearerTokenIntegrity.Output outputAuth;

            authDI = GetDI(httpContext.RequestServices);
            headers = context.HttpContext.Request.Headers;

            if (!headers.TryGetValue(AuthenticationHeader.Authorization, out StringValues authorizationHeader))
                throw new ApiException(StatusCodeEnum.UNAUTHORIZED);

            outputAuthTask = authDI.AuthenticationHelper.ValidateBearerTokenIntegrity(new ValidateBearerTokenIntegrity.Input()
            {
                BearerToken = authorizationHeader.ToString()
            });

            outputAuth = outputAuthTask.Result ?? throw new ApiException(StatusCodeEnum.UNAUTHORIZED);
            if (outputAuth.IsPublicUser) throw new ApiException(StatusCodeEnum.FORBIDDEN);

            authDI.Global.IdUser = outputAuth.IdUser;
            authDI.Global.IdProfile = outputAuth.IdProfile;
            authDI.Global.Uid = outputAuth.Uid;
            authDI.Global.Eid = outputAuth.Eid;
            authDI.Global.Token = authorizationHeader.ToString();
        }

        public static AuthPrivateDI GetDI(IServiceProvider serviceProvider)
        {
            return new AuthPrivateDI()
            {
                AuthenticationHelper = serviceProvider.GetService<AuthenticationHelper>() ?? throw new ArgumentNullException(nameof(AuthenticationHelper)),
                Global = serviceProvider.GetService<Global>() ?? throw new ArgumentNullException(nameof(Global)),
                Logger = serviceProvider.GetService<ILogger<PrivateIdentifierFilterAttribute>>() ?? throw new ArgumentNullException(nameof(ILogger<PrivateIdentifierFilterAttribute>))
            };
        }
    }

    public class AuthPrivateDI
    {
        public required AuthenticationHelper AuthenticationHelper { get; set; }
        public required Global Global { get; set; }
        public required ILogger<PrivateIdentifierFilterAttribute> Logger { get; set; }
    }
}
