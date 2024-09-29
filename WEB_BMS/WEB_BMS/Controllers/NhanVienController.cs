using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_BMS.Models;

namespace WEB_BMS.Controllers
{
    public class NhanVienController : Controller
    {
        // GET: NhanVien
        QuanLyVatLieuXayDungDataContext data = new QuanLyVatLieuXayDungDataContext();

        public ActionResult TrangChu()
        {
            return View();
        }

        //HienThiDSSP
        public ActionResult HienThiDSSP()
        {
            List<HangHoa> dssp = data.HangHoas.ToList();
            return View(dssp);
        }
        [HttpGet]
        public ActionResult ThemSPMoi()
        {
            ViewBag.MaLoai = new SelectList(data.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(data.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            return View();
        }
        [HttpPost]
        public ActionResult ThemSPMoi(HangHoa s, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(data.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(data.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");

            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/img/img_product"), filename);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    s.HinhAnh = filename;
                    data.HangHoas.InsertOnSubmit(s);
                    data.SubmitChanges();
                }
                return RedirectToAction("HienThiDSSP");
            }
        }
    }
}