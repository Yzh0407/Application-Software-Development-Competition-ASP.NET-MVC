using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 登录页面跳过 Session
            if (HttpContext.Request.Path == "/Home/Login")

            {
                return;
            }
            // 检查 Session
            if (HttpContext.Session.GetString("UserName") == null)
            {
                // Session 为空跳转到登录页面
                context.Result = RedirectToAction("Login", "Home");
            }
        }
    }
}
