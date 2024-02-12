using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
