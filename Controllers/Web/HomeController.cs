using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Web
{
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
