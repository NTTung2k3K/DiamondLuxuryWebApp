using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Session.GetString("token");
            if (token == null)
            {
                context.Result = new RedirectToActionResult("Index","Login",null);
            }
            base.OnActionExecuting(context);
        } 
    }
}
