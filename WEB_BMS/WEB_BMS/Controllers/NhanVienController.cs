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
            // Tải danh sách loại và nhà cung cấp
            ViewBag.MaLoai = new SelectList(data.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(data.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");

            // Kiểm tra xem mã sản phẩm có bị trùng không
            var existingSP = data.HangHoas.FirstOrDefault(h => h.MaHH == s.MaHH);
            if (existingSP != null)
            {
                // Nếu trùng mã sản phẩm, hiển thị thông báo lỗi bằng alert
                ViewBag.ThongBao1 = "<script>alert('Mã sản phẩm đã tồn tại!');</script>";
                return View(s); // Trả về view với dữ liệu đã nhập
            }

            // Kiểm tra ảnh
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh";
                return View(s); // Trả về view với dữ liệu đã nhập
            }

            if (ModelState.IsValid)
            {
                var filename = Path.GetFileName(fileupload.FileName);
                var path = Path.Combine(Server.MapPath("~/assets/img/img_product"), filename);

                if (System.IO.File.Exists(path))
                {
                    ViewBag.Thongbao = "Ảnh đã tồn tại";
                    return View(s); // Nếu ảnh đã tồn tại, trả về view và thông báo
                }
                else
                {
                    // Lưu ảnh vào thư mục
                    fileupload.SaveAs(path);
                }

                // Lưu tên ảnh vào cơ sở dữ liệu
                s.HinhAnh = filename;

                // Thêm sản phẩm mới vào cơ sở dữ liệu
                data.HangHoas.InsertOnSubmit(s);
                data.SubmitChanges();

                return RedirectToAction("HienThiDSSP"); // Chuyển hướng về danh sách sản phẩm sau khi thêm thành công
            }

            return View(s);
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

            // Kiểm tra nếu SoLuongTon là âm
            if (sp.SoLuongTon < 0)
            {
                ViewBag.Thongbao = "Số lượng tồn không thể là số âm.";
                return View(sp); // Trả về view với model
            }

            HangHoa sph = data.HangHoas.FirstOrDefault(t => t.MaHH == sp.MaHH);
            if (sph == null)
            {
                ViewBag.Thongbao = "Sản phẩm không tồn tại.";
                return View(sp); // Trả về view với model
            }

            // Nếu không có file upload, giữ nguyên hình ảnh hiện tại
            if (fileupload != null)
            {
                var filename = Path.GetFileName(fileupload.FileName);
                var path = Path.Combine(Server.MapPath("~/assets/img/img_product"), filename);

                // Lưu hình ảnh mới
                fileupload.SaveAs(path);
                sph.HinhAnh = filename; // Cập nhật hình ảnh
            }
            // Nếu không có file upload, giữ nguyên hình ảnh hiện tại

            // Cập nhật các thuộc tính khác
            sph.TenHangHoa = sp.TenHangHoa;
            sph.DonVi = sp.DonVi;
            sph.SoLuongTon = sp.SoLuongTon; // Cập nhật số lượng tồn
            sph.GiaBan = sp.GiaBan;
            sph.MaLoai = sp.MaLoai;
            sph.MaNCC = sp.MaNCC;

            data.SubmitChanges();

            return RedirectToAction("HienThiDSSP");
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


        ///CRUD LOAI
        public ActionResult HienThiDSLoai()
        {
            List<Loai> dsLoai = data.Loais.ToList();
            return View(dsLoai);
        }

        [HttpGet]
        public ActionResult ThemLoaiMoi()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ThemLoaiMoi(Loai loai)
        {
            // Kiểm tra xem mã loại có bị trùng không
            var existingLoai = data.Loais.FirstOrDefault(l => l.MaLoai == loai.MaLoai);
            if (existingLoai != null)
            {
                // Nếu trùng mã loại, hiển thị thông báo lỗi bằng alert
                ViewBag.ThongBao = "<script>alert('Mã loại đã tồn tại!');</script>";
                return View(loai); // Trả về view với dữ liệu đã nhập
            }

            if (ModelState.IsValid)
            {
                // Thêm loại mới vào cơ sở dữ liệu
                data.Loais.InsertOnSubmit(loai);
                data.SubmitChanges();

                return RedirectToAction("HienThiDSLoai"); // Chuyển hướng về danh sách loại sau khi thêm thành công
            }

            return View(loai); // Trả về view trong trường hợp ModelState không hợp lệ
        }


        public ActionResult SuaLoai(string id)
        {
            Loai loai = data.Loais.SingleOrDefault(n => n.MaLoai == id);
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);
        }

        [HttpPost]
        public ActionResult SuaLoai(Loai loai)
        {
            if (ModelState.IsValid)
            {
                Loai loaiCu = data.Loais.FirstOrDefault(l => l.MaLoai == loai.MaLoai);
                if (loaiCu != null)
                {
                    loaiCu.TenLoai = loai.TenLoai;
                    loaiCu.ThongTin = loai.ThongTin;
                    data.SubmitChanges();
                }
                return RedirectToAction("HienThiDSLoai");
            }
            return View(loai);
        }

        public ActionResult XoaLoai(string id)
        {
            Loai l = data.Loais.FirstOrDefault(t => t.MaLoai == id);

            if (l.HangHoas.Count == 0)
            {
                data.Loais.DeleteOnSubmit(l);
                data.SubmitChanges();
                return RedirectToAction("HienThiDSLoai");
            }
            ViewBag.tb = "Loại này đang có sản phẩm, Không xóa được!!!";
            return View(l);
        }

        ///CRUD NHA CUNG CAP
        public ActionResult HienThiDSNCC()
        {
            List<NhaCungCap> dsNCC = data.NhaCungCaps.ToList();
            return View(dsNCC);
        }

        [HttpGet]
        public ActionResult ThemNCCMoi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemNCCMoi(NhaCungCap ncc)
        {
            // Kiểm tra xem mã NCC có bị trùng không
            var existingNCC = data.NhaCungCaps.FirstOrDefault(n => n.MaNCC == ncc.MaNCC);

            if (existingNCC != null)
            {
                // Nếu trùng mã NCC, hiển thị thông báo lỗi bằng alert
                ViewBag.ThongBao = "<script>alert('Mã Nhà Cung Cấp đã tồn tại!');</script>";
                return View(ncc); // Trả về view với dữ liệu đã nhập
            }

            // Nếu không trùng, tiếp tục thêm mới
            if (ModelState.IsValid)
            {
                data.NhaCungCaps.InsertOnSubmit(ncc);
                data.SubmitChanges();
                return RedirectToAction("HienThiDSNCC"); // Chuyển hướng về danh sách NCC sau khi thêm thành công
            }

            return View(ncc);
        }


        public ActionResult SuaNCC(string id)
        {
            NhaCungCap ncc = data.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }

        [HttpPost]
        public ActionResult SuaNCC(NhaCungCap ncc)
        {
            if (ModelState.IsValid)
            {
                NhaCungCap nccCu = data.NhaCungCaps.FirstOrDefault(n => n.MaNCC == ncc.MaNCC);
                if (nccCu != null)
                {
                    nccCu.TenNCC = ncc.TenNCC;
                    nccCu.Diachi = ncc.Diachi;
                    nccCu.SDT = ncc.SDT;
                    data.SubmitChanges();
                }
                return RedirectToAction("HienThiDSNCC");
            }
            return View(ncc);
        }

        public ActionResult XoaNCC(string id)
        {
            NhaCungCap l = data.NhaCungCaps.FirstOrDefault(t => t.MaNCC == id);

            if (l.HangHoas.Count == 0)
            {
                data.NhaCungCaps.DeleteOnSubmit(l);
                data.SubmitChanges();
                return RedirectToAction("HienThiDSNCC");
            }
            ViewBag.tb = "Nhà cung cấp này hiện vẫn đang cung cấp sản phẩm. Không xóa được!!!";
            return View(l);
        }

        public ActionResult HienThiDSDonNhanHang()
        {
            //var hangHoas = data.HangHoas.Where(t => t.MaNCC == mncc).ToList();
            return View();
        }
    }
}