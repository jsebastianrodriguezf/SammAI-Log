using Microsoft.AspNetCore.Diagnostics;
using SAMMAI.Transverse.Constants;
using SAMMAI.Transverse.Helpers;

namespace SAMMAI.Log.Utility.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            IExceptionHandlerPathFeature? contextFeature;
            string messageError;
            object? bodyError;

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)StatusCodeEnum.INTERNAL_SERVER_ERROR;
                    context.Response.ContentType = GeneralConstants.ContentType.Json;

                    contextFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    if (contextFeature != null)
                    {
                        app.Logger.LogError("Something went wrong: {Servicio} | {Error}", "SAMMAI Log API", contextFeature.Error);

                        if (contextFeature.Error is ApiException apiException)
                        {
                            context.Response.StatusCode = (int)apiException.StatusCode;
                            messageError = apiException.Message;
                            bodyError = apiException.BodyError;
                        }
                        else if (contextFeature.Error.InnerException is ApiException apiInnerException)
                        {
                            context.Response.StatusCode = (int)apiInnerException.StatusCode;
                            messageError = apiInnerException.Message;
                            bodyError = apiInnerException.BodyError;
                        }
                        else
                        {
                            messageError = contextFeature.Error.Message;
                            bodyError = null;
                        }

                        if (bodyError is null)
                            await context.Response.WriteAsync(ResponseHelper.SetResponse(context.Response.StatusCode, messageError).ToJson(true));
                        else
                            await context.Response.WriteAsync(ResponseHelper.SetBadRequestResponseWithError(bodyError, messageError).ToJson(true));
                    }
                });
            });
        }
    }
}
