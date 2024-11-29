using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // 如果是登录页面，跳过 Session 检查
        if (HttpContext.Request.Path == "/Home/Login")
        {
            return;
        }
        // 检查 Session 中是否有用户名
        if (HttpContext.Session.GetString("Username") == null)
        {
            // 如果用户名为空，跳转到登录页面
            context.Result = RedirectToAction("Login", "Home");
        }
    }
}