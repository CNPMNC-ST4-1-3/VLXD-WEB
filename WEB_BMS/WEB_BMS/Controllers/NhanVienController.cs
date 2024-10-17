using System;
using System.Collections.Generic;
using System.Data.Linq;
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

        //ThemSP
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

        //SuaSP
        public ActionResult SuaSP(string id)
        {

            ViewBag.MaLoai = new SelectList(data.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(data.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");

            HangHoa hh = data.HangHoas.SingleOrDefault(n => n.MaHH == id);
            if (hh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hh);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSP(HangHoa sp, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(data.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(data.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");

            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
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

                    HangHoa sph = data.HangHoas.FirstOrDefault(t => t.MaHH == sp.MaHH);
                    if (sph != null)
                    {
                        sph.TenHangHoa = sp.TenHangHoa;
                        sph.DonVi = sp.DonVi;
                        sph.SoLuongTon = sp.SoLuongTon;
                        sph.HinhAnh = filename;
                        sph.GiaBan = sp.GiaBan;
                        sph.MaLoai = sp.MaLoai;
                        sph.MaNCC = sp.MaNCC;


                        data.SubmitChanges();
                    }
                }

                return RedirectToAction("HienThiDSSP");
            }
        }
        //XoaSP
        public ActionResult XoaSP(string id)
        {

            HangHoa hh = data.HangHoas.SingleOrDefault(n => n.MaHH == id);
            ViewBag.MaHH = hh.MaHH;
            if (hh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hh);

        }
        public ActionResult Xacnhanxoa(string id)
        {

            HangHoa hh = data.HangHoas.FirstOrDefault(n => n.MaHH == id);
            if (hh != null && hh.ChiTietDonBanHangs == null && hh.ChiTietDonNhapHangs == null)
            {
                // Khởi tạo `ChiTietDonBanHangs`
                hh.ChiTietDonBanHangs = new EntitySet<ChiTietDonBanHang>();
                hh.ChiTietDonBanHangs.AddRange(hh.ChiTietDonBanHangs);

                // Khởi tạo `ChiTietDonNhapHangs`
                hh.ChiTietDonNhapHangs = new EntitySet<ChiTietDonNhapHang>();
                hh.ChiTietDonNhapHangs.AddRange(hh.ChiTietDonNhapHangs);
            }
            if (hh != null && hh.ChiTietDonBanHangs != null && hh.ChiTietDonBanHangs.Count == 0 && hh.ChiTietDonNhapHangs != null && hh.ChiTietDonNhapHangs.Count == 0)
            {
                data.HangHoas.DeleteOnSubmit(hh);
                data.SubmitChanges();
                ViewBag.tb = "Xóa thành công!!!";
                return View(hh);
            }
            ViewBag.tb = "Sản phẩm đang có trong đơn nhập hàng hoặc bán hàng. Vui lòng kiểm tra lại!!!";
            return View(hh);
        }
    }
}