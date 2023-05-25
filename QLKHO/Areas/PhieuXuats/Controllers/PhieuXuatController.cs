using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLKHO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLKHO.Areas.PhieuXuats.Controllers
{
    [Area("PhieuXuats")]
    [Route("/[controller]/[action]")]
    [Authorize(Roles = "Admin,NhanVien")]
    public class PhieuXuatController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public PhieuXuatController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _context.phieuXuats.ToListAsync());
        }
        public class InputModel
        {
            public List<SanPham> sanPhams { get; set; }
            public SelectList khachHang { get; set; }
            [Required(ErrorMessage = "{0} không được để trống")]
            [DisplayName("Khách Hàng")]
            public int MaKh { get; set; }
            [DisplayName("Ngày Lập Phiếu")]
            [Required(ErrorMessage = "{0} không được để trống")]
            public DateTime NgayLap { get; set; } = DateTime.Now.Date;
            [Required(ErrorMessage = "Đơn giá không được để trống")]
            [Range(0, int.MaxValue, ErrorMessage = "Đơn giá phải có giá trị từ {1} trở lên")]
            public decimal[] DonGia { get; set; }
            [Required(ErrorMessage = "Số lượng không được để trống")]
            [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải có giá trị từ {1} đến số lượng của sản phẩm đó")]
            public int[] SoLuong { get; set; }
            [Required(ErrorMessage = "Phải chọn sản phẩm")]
            public int[] MaSp { get; set; }

        }
        public async Task<IActionResult> Create()
        {
            InputModel inputModel = new InputModel()
            {
                sanPhams = await _context.sanPhams.ToListAsync(),
                khachHang = new SelectList(_context.khachHangs,"MaKh","TenKh")
            };
            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKh,NgayLap,DonGia,SoLuong,MaSp")]InputModel inputModel)
        {
            if (inputModel.MaSp == null || inputModel.SoLuong == null || inputModel.DonGia == null)
            {
                TempData["thongbao"] = $"Error Tạo phiếu thất bại do chưa nhập đủ dữ liệu";
                InputModel input = new InputModel()
                {
                    NgayLap = inputModel.NgayLap,
                    sanPhams = await _context.sanPhams.ToListAsync(),
                    khachHang = new SelectList(_context.khachHangs, "MaKh", "TenKh",inputModel.MaKh)
                };
                return View(input);
            }
            if (inputModel.MaSp.Count() != inputModel.SoLuong.Count() || inputModel.MaSp.Count() != inputModel.DonGia.Count())
            {
                TempData["thongbao"] = $"Error Tạo phiếu thất bại do chưa nhập đủ dữ liệu";
                InputModel input = new InputModel()
                {
                    NgayLap = inputModel.NgayLap,
                    sanPhams = await _context.sanPhams.ToListAsync(),
                    khachHang = new SelectList(_context.khachHangs, "MaKh", "TenKh", inputModel.MaKh)
                };
                return View(input);
            }
            for (int i = 0; i < inputModel.MaSp.Count(); i++)
            {
                if (inputModel.DonGia[i] < 0)
                {
                    TempData["thongbao"] = $"Error Tạo phiếu thất bại do Đơn Giá nhập sai giá trị";
                    InputModel input = new InputModel()
                    {
                        NgayLap = inputModel.NgayLap,
                        sanPhams = await _context.sanPhams.ToListAsync(),
                        khachHang = new SelectList(_context.khachHangs, "MaKh", "TenKh", inputModel.MaKh)
                    };
                    return View(input);
                }
                SanPham sp = _context.sanPhams.SingleOrDefault(s => s.MaSp == inputModel.MaSp[i]);
                if(sp == null)
                {
                    return NotFound("Không tìm thấy các sản phẩm cần thêm");
                }
                else
                {
                    if(sp.SoLuongCo < inputModel.SoLuong[i] || inputModel.SoLuong[i] < 1)
                    {
                        TempData["thongbao"] = $"Error Tạo phiếu thất bại do số lượng của sản phẩm có mã {sp.MaSp} nhập sai" +
                            $"(số lượng phải lớn hơn 0 và nhỏ hơn hoặc bằng số lượng có của sản phẩm)";
                        InputModel input = new InputModel()
                        {
                            NgayLap = inputModel.NgayLap,
                            sanPhams = await _context.sanPhams.ToListAsync(),
                            khachHang = new SelectList(_context.khachHangs, "MaKh", "TenKh", inputModel.MaKh)
                        };
                        return View(input);
                    }
                }
            }
            int tongsoluong = 0;
            decimal tongtien = 0;
            for(int i = 0;i < inputModel.MaSp.Count(); i++)
            {
                tongsoluong += inputModel.SoLuong[i];
                tongtien += inputModel.DonGia[i] * inputModel.SoLuong[i];
            }
            try
            {
                PhieuXuat phieuXuat = new PhieuXuat()
                {
                    NgayLap = inputModel.NgayLap,
                    MaKh = inputModel.MaKh,
                    UserId = _userManager.GetUserId(User),
                    TongSoLuong = tongsoluong,
                    TongTien= tongtien,
                };
                _context.Add(phieuXuat);
                await _context.SaveChangesAsync();
                for (int i = 0; i < inputModel.MaSp.Count(); i++)
                {
                    ChiTietPhieuXuat ctpx = new ChiTietPhieuXuat()
                    {
                        MaPX = phieuXuat.MaPX,
                        MaSp = inputModel.MaSp[i],
                        SoLuong = inputModel.SoLuong[i],
                        DonGia = inputModel.DonGia[i],
                    };
                    _context.Add(ctpx);
                    SanPham sanPham = await _context.sanPhams.SingleOrDefaultAsync(sp => sp.MaSp == inputModel.MaSp[i]);
                    if(sanPham == null)
                    {
                        return NotFound("Tạo phiếu xuất thất bại do không tìm thấy sản phẩm cần thêm");
                    }
                    sanPham.SoLuongCo = sanPham.SoLuongCo - inputModel.SoLuong[i];
                    _context.sanPhams.Update(sanPham);
                }
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                TempData["thongbao"] = "Error Tạo phiếu xuất thất bại";
                InputModel input = new InputModel()
                {
                    NgayLap = inputModel.NgayLap,
                    sanPhams = await _context.sanPhams.ToListAsync(),
                    khachHang = new SelectList(_context.khachHangs, "MaKh", "TenKh", inputModel.MaKh)
                };
                return View(input);
            }
            TempData["thongbao"] = "Tạo phiếu nhập thành công";
            return RedirectToAction(nameof(Index));
        }
        public class InputDetails
        {
            public PhieuXuat phieuXuat { get; set; }
            public int MaPx { get; set; }
            public List<SP_PX>  sanPham_phieuXuat { get; set; }
            public AppUser user { get; set; }
            public KhachHang khachHang { get; set; }
        }
        public class SP_PX
        {
            public int MaSp { get; set; }
            public string TenSp { get; set; }
            public string Photo { get; set; }
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound("Không tìm thấy mã phiếu");
            }
            PhieuXuat phieuXuat = await _context.phieuXuats.SingleOrDefaultAsync(pn => pn.MaPX == id);
            if (phieuXuat == null)
            {
                return NotFound("Không tìm thấy phiếu xuất");
            }
            var sp_px = from sp in _context.sanPhams
                        join ct in _context.chiTietPhieuXuats
                        on sp.MaSp equals ct.MaSp
                        where ct.MaPX == phieuXuat.MaPX
                        select new SP_PX
                        {
                            MaSp = sp.MaSp,
                            TenSp = sp.TenSp,
                            Photo = sp.Photo,
                            SoLuong = ct.SoLuong,
                            DonGia = ct.DonGia
                        };
            InputDetails inputDetails = new InputDetails()
            {
                phieuXuat = phieuXuat,
                MaPx = phieuXuat.MaPX,
                sanPham_phieuXuat = sp_px.ToList(),
                khachHang = await _context.khachHangs.SingleOrDefaultAsync(kh => kh.MaKh == phieuXuat.MaKh),
                user = await _userManager.FindByIdAsync(phieuXuat.UserId)
            };
            return View(inputDetails);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound("Xoa thất bại do không tìm thấy mã phiếu");
            }
            PhieuXuat phieuXuat = await _context.phieuXuats.SingleOrDefaultAsync(ph => ph.MaPX == id);
            if(phieuXuat == null)
            {
                return NotFound("không tìm thấy phiếu");
            }
            try
            {
                _context.phieuXuats.Remove(phieuXuat);
                List<ChiTietPhieuXuat> ctpx = await _context.chiTietPhieuXuats.Where(ct => ct.MaPX == phieuXuat.MaPX).ToListAsync();
                foreach(var item in ctpx)
                {
                    SanPham sp = await _context.sanPhams.SingleOrDefaultAsync(s => s.MaSp == item.MaSp);
                    if(sp == null)
                    {
                        return NotFound("Xóa thất bại do không tìm thấy sản phẩm trong phiếu");
                    }
                    sp.SoLuongCo = sp.SoLuongCo + item.SoLuong;
                    _context.sanPhams.Update(sp);
                }
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                TempData["thongbao"] = $"Error Xóa phiếu có mã {phieuXuat.MaPX} bị thất bại";
                return RedirectToAction(nameof(Index));
            }
            TempData["thongbao"] = $"Xóa phiếu có mã {phieuXuat.MaPX} thành công";
            return RedirectToAction(nameof(Index));
        }
    }
}
