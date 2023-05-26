﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLKHO.Models;

namespace QLKHO.Areas.KhachHangs.Controllers
{
    [Route("/[controller]/[action]")]
    [Area("KhachHangs")]
    [Authorize(Roles = "Admin,NhanVien")]
    public class KhachHangController : Controller
    {
        private readonly AppDbContext _context;

        public KhachHangController(AppDbContext context)
        {
            _context = context;
        }

        // GET: KhachHangs/KhachHang
        public async Task<IActionResult> Index()
        {
            return View(await _context.khachHangs.ToListAsync());
        }

        // GET: KhachHangs/KhachHang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.khachHangs
                .SingleOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null)
            {
                return NotFound("không tìm thấy khách hàng");
            }

            return View(khachHang);
        }

        // GET: KhachHangs/KhachHang/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KhachHangs/KhachHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKh,TenKh,Email,Phone")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(khachHang);
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    TempData["thongbao"] = "Error Tạo thất bại";
                    return View(khachHang);
                }
                TempData["thongbao"] = "Tạo Khách Hàng thành công";
                return RedirectToAction(nameof(Index));
            }
            TempData["thongbao"] = "Error Tạo thất bại do chưa nhập đúng dữ liệu";
            return View(khachHang);
        }

        // GET: KhachHangs/KhachHang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.khachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        // POST: KhachHangs/KhachHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("MaKh,TenKh,Email,Phone")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.MaKh))
                    {
                        return NotFound("khách hàng này không tồn tại");
                    }
                    else
                    {
                        TempData["thongbao"] = $"Error khách hàng thất bại";
                        return View(khachHang) ;
                    }
                }
                TempData["thongbao"] = $"Bạn đã sửa thành công khách hàng có mã {khachHang.MaKh}";
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: KhachHangs/KhachHang/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.khachHangs
                .FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // POST: KhachHangs/KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if(id == null)
            {
                return NotFound("Không xóa được khách hàng");
            }
            KhachHang khachHang = await _context.khachHangs.SingleOrDefaultAsync(k => k.MaKh == id);
            if(khachHang == null)
            {
                return NotFound("Không xóa được do không tìm thấy khách hàng");
            }
            try
            {
                _context.khachHangs.Remove(khachHang);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["thongbao"] = $"Error không xóa được khách hàng có mã {khachHang.MaKh}";
                return RedirectToAction(nameof(Index));
            }
            TempData["thongbao"] = $"Xóa khách hàng thành công";
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
            return _context.khachHangs.Any(e => e.MaKh == id);
        }
    }
}