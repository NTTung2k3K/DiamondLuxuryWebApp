using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiamondLuxurySolution.WebApp.Models
{
    public class InternalRedirectAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var referer = context.HttpContext.Request.Headers["Referer"].ToString();

            // Check if the referer is from the same host
            if (string.IsNullOrEmpty(referer) || !referer.Contains(context.HttpContext.Request.Host.Value))
            {
                context.Result = new RedirectToActionResult("Unauthorized", "Home", null);
            }
        }
    }
}
