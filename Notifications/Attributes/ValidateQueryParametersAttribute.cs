using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;

namespace Notifications.Attributes
{
    /// <summary>
    /// Action filter that verifies that no additional parameters are passed in the query string
    /// beyond those that we specified in the controller method's signatures.
    /// Returns 400 Bad Request.
    /// </summary>
    public class ValidateQueryParametersAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// This method runs before every WS invocation
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var expectedParameters = actionContext.ActionDescriptor.Parameters;
            var queryParameters = actionContext.HttpContext.Request.Query;

            if (queryParameters.Select(kvp => kvp.Key).Any(queryParameter => !expectedParameters.Any(p => p.Name == queryParameter)))
            {
                actionContext.Result = new BadRequestObjectResult("Bad request - check query parameters");
            }
        }
    }
}