using Microsoft.AspNetCore.Mvc;

namespace HomeTasksApp.Controllers
{
    public class BaseController : Controller
    {
        protected int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        protected bool IsUserLoggedIn()
        {
            return GetCurrentUserId().HasValue;
        }
    }
}
