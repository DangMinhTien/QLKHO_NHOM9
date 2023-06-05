using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKHO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ThongKeController(AppDbContext context, 
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public class BaoCao
        {
            public List<BaoCaoVon> baoCaoVons { get; set; }
            public List<BaoCaoDoanhThu> baoCaoDoanhThus { get; set; }
            public int? searchYear { get; set; }
        }
        public class BaoCaoVon
        {
            public int thang { get; set; }
            public decimal TongTien { get; set; }
        }
        public class BaoCaoDoanhThu
        {
            public int thang { get; set; }
            public decimal TongTien { get; set; }
        }
        public async Task<IActionResult> Index(int? searchYear)
        {
            ViewData["soluongton"] = await _context.sanPhams.SumAsync(sp => sp.SoLuongCo);
            ViewData["von"] = await _context.phieuNhaps.SumAsync(pn => pn.TongTien);
            ViewData["doanhthu"] = await _context.phieuXuats.SumAsync(px => px.TongTien);
            ViewData["lai"] = (ViewBag.doanhthu - ViewBag.von > 0) ?
                               ViewBag.doanhthu - ViewBag.von : 0;
            if(searchYear == null)
                searchYear = DateTime.Now.Year;

            var bcvon = (from pnhap in _context.phieuNhaps
                        where pnhap.NgayLap.Year == searchYear
                        group pnhap by pnhap.NgayLap.Month into lstpnhap
                        select new BaoCaoVon
                        {
                            thang = lstpnhap.Key,
                            TongTien = lstpnhap.ToList().Sum(x => x.TongTien)
                        }).ToList();
            var bcdoanhthu = (from pxuat in _context.phieuXuats
                         where pxuat.NgayLap.Year == searchYear
                         group pxuat by pxuat.NgayLap.Month into lstpxuat
                              select new BaoCaoDoanhThu
                         {
                             thang = lstpxuat.Key,
                             TongTien = lstpxuat.ToList().Sum(x => x.TongTien)
                         }).ToList();
            List<int> monthsInYear = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12 };
            foreach (var month in monthsInYear)
            {
                var von = bcvon.FirstOrDefault(bc => bc.thang == month);
                if(von == null)
                {
                    bcvon.Add(new BaoCaoVon { thang = month, TongTien = 0 });
                }
                var doanhthu = bcdoanhthu.FirstOrDefault(bc => bc.thang == month);
                if(doanhthu == null)
                {
                    bcdoanhthu.Add(new BaoCaoDoanhThu { thang = month, TongTien = 0});
                }
            }
            
            BaoCao baocao = new BaoCao() 
            { 
                baoCaoVons = bcvon.OrderBy(bc => bc.thang).ToList(), 
                baoCaoDoanhThus = bcdoanhthu.OrderBy(bc => bc.thang).ToList(),
                searchYear = searchYear,
            };
            return View(baocao);
        }
    }
}
