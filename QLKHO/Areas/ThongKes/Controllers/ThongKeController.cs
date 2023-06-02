using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKHO.Models;
using System.Threading.Tasks;

namespace QLKHO.Areas.ThongKes.Controllers
{
    [Area("ThongKes")]
    [Route("/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class ThongKeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ThongKeController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["soluongton"] = await _context.sanPhams.SumAsync(sp => sp.SoLuongCo);
            ViewData["von"] = await _context.phieuNhaps.SumAsync(pn => pn.TongTien);
            ViewData["doanhthu"] = await _context.phieuXuats.SumAsync(px => px.TongTien);
            ViewData["lai"] = (ViewBag.doanhthu - ViewBag.von > 0) ?
                               ViewBag.doanhthu - ViewBag.von : 0;
            return View();
        }
    }
}
