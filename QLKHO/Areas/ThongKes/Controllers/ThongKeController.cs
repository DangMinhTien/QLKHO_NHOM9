using Microsoft.AspNetCore.Mvc;

namespace QLKHO.Areas.ThongKes.Controllers
{
    [Area("ThongKes")]
    [Route("/[controller]/[action]")]
    public class ThongKeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
