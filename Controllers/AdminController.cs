using Microsoft.AspNetCore.Mvc;

namespace DVMS.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult companies()
        {
            return View();
        }
        public IActionResult employee()
        {
            return View();
        }
        public IActionResult inviteguest()
        {
            return View();
        }
        public IActionResult trackguest()
        {
            return View();
        }
        public IActionResult trackvisitor()
        {
            return View();
        }
        public IActionResult Home()
        {
            return View();
        }
    }
}
