using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using QLKHO.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        public BaoCao CreateBaoCao(int? year)
        {
            var bcvon = (from pnhap in _context.phieuNhaps
                         where pnhap.NgayLap.Year == year
                         group pnhap by pnhap.NgayLap.Month into lstpnhap
                         select new BaoCaoVon
                         {
                             thang = lstpnhap.Key,
                             TongTien = lstpnhap.ToList().Sum(x => x.TongTien)
                         }).ToList();
            var bcdoanhthu = (from pxuat in _context.phieuXuats
                              where pxuat.NgayLap.Year == year
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
                if (von == null)
                {
                    bcvon.Add(new BaoCaoVon { thang = month, TongTien = 0 });
                }
                var doanhthu = bcdoanhthu.FirstOrDefault(bc => bc.thang == month);
                if (doanhthu == null)
                {
                    bcdoanhthu.Add(new BaoCaoDoanhThu { thang = month, TongTien = 0 });
                }
            }

            BaoCao baocao = new BaoCao()
            {
                baoCaoVons = bcvon.OrderBy(bc => bc.thang).ToList(),
                baoCaoDoanhThus = bcdoanhthu.OrderBy(bc => bc.thang).ToList(),
                searchYear = year,
            };
            return baocao;
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
            BaoCao baocao = CreateBaoCao(searchYear);

            return View(baocao);
        }
        public IActionResult ExportExcel(int? year)
        {
            if (year == null)
                year = DateTime.Now.Year;
            BaoCao baocao = CreateBaoCao(year);
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("DanhThu");
                //đổ header cho sheet
                sheet.Cells[1, 1].Value = "Tháng"; 
                sheet.Cells[1, 2].Value = "Vốn bỏ ra";
                sheet.Cells[1, 3].Value = "Doanh Thu";
                sheet.Cells[1, 4].Value = "Lãi";
                //đổ dữ liệu vào sheet
                int rowIndex = 2;
                for(int i = 0;i < baocao.baoCaoVons.Count(); i++)
                {
                    sheet.Cells[rowIndex, 1].Value = $"Tháng {baocao.baoCaoVons[i].thang}";
                    sheet.Cells[rowIndex, 2].Value = baocao.baoCaoVons[i].TongTien;
                    sheet.Cells[rowIndex, 3].Value = baocao.baoCaoDoanhThus[i].TongTien;
                    sheet.Cells[rowIndex, 4].Value = baocao.baoCaoDoanhThus[i].TongTien - baocao.baoCaoVons[i].TongTien;
                    rowIndex++;
                }
                sheet.Cells[rowIndex, 1].Value = $"Tổng Năm {year}";
                sheet.Cells[rowIndex, 2].Value = baocao.baoCaoVons.Sum(bc => bc.TongTien);
                sheet.Cells[rowIndex, 3].Value = baocao.baoCaoDoanhThus.Sum(bc => bc.TongTien);
                sheet.Cells[rowIndex, 4].Value = baocao.baoCaoDoanhThus.Sum(bc => bc.TongTien) - baocao.baoCaoVons.Sum(bc => bc.TongTien);
                package.Save();
            }
            stream.Position = 0;
            var fileName = $"Bao_cao_Kinh_Doanh_Nam{year}.xlsx";
            return File(stream, "application/vnd.openxmlformats-" +
                "officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
