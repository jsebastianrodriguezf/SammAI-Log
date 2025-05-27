using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SAMMAI.Transverse.Helpers;
using SAMMAI.Transverse.Models.Error;

namespace SAMMAI.Log.Utility.ActionFilters
{
    public class FluentValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;

            List<InvalidFieldModel> invalidFieldModels = [];

            foreach (string field in context.ModelState.Keys)
            {
                ModelStateEntry? modelState = context.ModelState.FirstOrDefault(x => x.Key.Equals(field)).Value;

                if (modelState is null) continue;

                invalidFieldModels.Add(new InvalidFieldModel()
                {
                    Field = field,
                    Errors = modelState.Errors.Select(x => x.ErrorMessage).ToList()
                });
            }

            context.Result = new JsonResult(ResponseHelper.SetBadRequestResponseWithError(invalidFieldModels, "One or more fields are wrong"))
            {
                StatusCode = (int)StatusCodeEnum.BAD_REQUEST
            };
        }
    }
}
