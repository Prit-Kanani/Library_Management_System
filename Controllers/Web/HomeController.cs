using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Web
{
    [Authorize(Policy = "AdminOrLibrarian")]
    public class HomeController : Controller
    {
        #region Views

        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
