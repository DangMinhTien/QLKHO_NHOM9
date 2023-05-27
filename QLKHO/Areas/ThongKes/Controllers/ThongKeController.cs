using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace QLKHO.Areas.ThongKes.Controllers
{
    [Area("ThongKes")]
    [Route("/[controller]/[action]")]
    public class ThongKeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
