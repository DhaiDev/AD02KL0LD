using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MoviesAPI.Filter
{
    public class ParseBadRequest : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result as IStatusCodeActionResult ;
            if (result == null)
            {
                return;
            }

            var statusCode = result.StatusCode ;

            if (statusCode == 400) {
                var respone = new List<string>();
                var badRequestObjectResult = context.Result as BadRequestObjectResult ;
                if (badRequestObjectResult.Value is string)
                {
                    respone.Add(badRequestObjectResult.Value.ToString());
                }
                else
                {
                    foreach (var key in context.ModelState.Keys)
                    {
                        foreach (var error in context.ModelState[key].Errors)
                        {
                            respone.Add($"{key}: {error.ErrorMessage}" );
                        }
                    }
                }
                context.Result = new BadRequestObjectResult(respone);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
