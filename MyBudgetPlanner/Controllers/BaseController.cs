using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyBudgetPlanner.Controllers
{
    public class BaseController : Controller
    {

        public string GetLoggedInUserId()
        {
            string? LoggedInUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(LoggedInUserId))
                return string.Empty;
            else return LoggedInUserId;

        }
    }
}
