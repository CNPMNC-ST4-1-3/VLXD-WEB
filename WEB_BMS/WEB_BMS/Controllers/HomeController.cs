using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using WEB_BMS.Models;

namespace WEB_BMS.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        QuanLyVatLieuXayDungDataContext data = new QuanLyVatLieuXayDungDataContext();
        public ActionResult Index()
        {
            List<HangHoa> dshh = data.HangHoas.ToList();
            return View(dshh);
        }
        
        public ActionResult Introduce()
        {
            return View();
        }

        public async Task<ActionResult> Product(string id)
        {
            // Danh Gia
            IFirebaseClient client = CreateFirebaseClient();

            // Fetch existing reviews from Firebase
            List<DanhGia> dsdanhgia = await GetThongTinKhoa("DanhGia");

            // Create a model to hold reviews and form data
            var model = new DanhGia
            {
                Reviews = dsdanhgia
            };

            // Pass the model to the view
            ViewBag.Reviews = model;

            // Hang Hoa
            HangHoa hh = data.HangHoas.SingleOrDefault(n => n.MaHH == id);
            var currentProduct = data.HangHoas.SingleOrDefault(p => p.MaHH == id);

            if (currentProduct == null)
            {
                return HttpNotFound();
            }

            // Retrieve similar products excluding the current one
            var similarProducts = data.HangHoas.Where(p => p.MaHH != id).ToList();

            // Pass both the current product and the list of similar products to the view
            ViewBag.lstHH = similarProducts;
            return View(hh);
        }
        // POST: DanhGia
        [HttpPost]
        public async Task<ActionResult> Product(string id, FormCollection col)
        {
            string ten = col["HoTen"];
            string bl = col["BinhLuan"];
            // Initialize Firebase client
            IFirebaseClient client = CreateFirebaseClient();

            // Fetch existing reviews from Firebase
            List<DanhGia> dsdanhgia = await GetThongTinKhoa("DanhGia");

            // Calculate the new entry ID
            string stt = (dsdanhgia.Count + 1).ToString();

            // Create a new DanhGia object from form data
            DanhGia themdg = new DanhGia
            {
                HoTen = ten,
                BinhLuan = bl
            };

            // Insert the new DanhGia into Firebase
            await FirebaseInsertData(client, themdg, "DanhGia/" + stt + "/");

            // Re-fetch the updated list of reviews
            dsdanhgia = await GetThongTinKhoa("DanhGia");

            // Create a model to hold reviews and form data
            var updatedModel = new DanhGia
            {
                Reviews = dsdanhgia
            };

            // Pass the updated model to the view

            ViewBag.Reviews = updatedModel;

            // Hang Hoa
            HangHoa hh = data.HangHoas.SingleOrDefault(n => n.MaHH == id);
            var currentProduct = data.HangHoas.SingleOrDefault(p => p.MaHH == id);

            if (currentProduct == null)
            {
                return HttpNotFound();
            }

            // Retrieve similar products excluding the current one
            var similarProducts = data.HangHoas.Where(p => p.MaHH != id).ToList();

            // Pass both the current product and the list of similar products to the view
            ViewBag.lstHH = similarProducts;
            return View(hh);
        }

       

     
        

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult New_NH()
        {
            return View();
        }
        public ActionResult New_TT()
        {
            return View();
        }

        public ActionResult Table_Price()
        {
            List<HangHoa> dshh = data.HangHoas.ToList();
            return View(dshh);
        }

        public ActionResult Product_Index(int page = 1)
        {
            // Lấy tất cả sản phẩm
            var products = data.HangHoas.AsQueryable();

            // Lọc sản phẩm theo giá

            List<HangHoa> dshh = data.HangHoas.ToList();
            // paging
            int NoOfRecordPerPage = 6;
            int NoOfPages = Convert.ToInt32(Math.Ceiling((Convert.ToDouble(dshh.Count) / Convert.ToDouble(NoOfRecordPerPage))));

            int NoOfRecordToSKip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;

            dshh = dshh.Skip(NoOfRecordToSKip).Take(NoOfRecordPerPage).ToList();
            return View(dshh);
        }
        [HttpPost]
        public ActionResult Product_Index(int page = 1, decimal? minPrice = null, decimal? maxPrice = null, string sortOrder = "asc")
        {
            // Lấy tất cả sản phẩm và chuyển đổi thành IQueryable để có thể thực hiện các thao tác lọc và phân trang
            var products = data.HangHoas.AsQueryable();

            // Lọc sản phẩm theo giá
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.GiaBan >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.GiaBan <= maxPrice.Value);
            }

            // Sắp xếp sản phẩm theo giá
            products = sortOrder == "asc" ? products.OrderBy(p => p.GiaBan) : products.OrderByDescending(p => p.GiaBan);

            // Phân trang
            int NoOfRecordPerPage = 6;
            int NoOfPages = Convert.ToInt32(Math.Ceiling((Convert.ToDouble(products.Count()) / Convert.ToDouble(NoOfRecordPerPage))));

            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortOrder = sortOrder;

            var pagedProducts = products.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            return View(pagedProducts);
        }
       

        public ActionResult HTDSTheoLoai(string id)
        {
            List<HangHoa> dssp = data.HangHoas.Where(n => n.MaLoai == id).ToList();
            return View("Product_Index", dssp);
        }

        public ActionResult Login(string id)
        {
            List<HangHoa> dssp = data.HangHoas.Where(n => n.MaLoai == id).ToList();
            return View("Product_Index", dssp);
        }
        //Search
        [HttpPost]
        public ActionResult Search(FormCollection c)
        {
            string text = c["id"].ToString();
            List<HangHoa> ds = data.HangHoas.Where(t => t.TenHangHoa.Contains(text)).ToList();
            if (ds.Count() == 0)
            {
                ViewBag.tb = "Không tồn tại sản phẩm bạn muốn tìm.";
                return View("Product_Index", new List<HangHoa>());
            }
            else
            {
                return View("Product_Index", ds);
            }
        }
      
       
        public ActionResult GioHangPartial()
        {
            var cart = GetCart();
            ViewBag.TongSoLuong = cart.GetItemCount();
            return PartialView();
        }
        //Giỏ Hàng
        private ShoppingCart GetCart()
        {
            var cart = Session["Cart"] as ShoppingCart;
            if (cart == null)
            {
                cart = new ShoppingCart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public HangHoa GetProductById(string productId)
        {
            return data.HangHoas.SingleOrDefault(n => n.MaHH == productId);
        }
        // Thêm giỏ hàng
        public ActionResult AddToCart(string productId)
        {
            var cart = GetCart();

            // Lấy thông tin sản phẩm từ cơ sở dữ liệu (ví dụ)
            HangHoa product = GetProductById(productId);
            if (product != null)
            {
                var cartItem = new CardItems
                {
                    MaHH = productId,
                    TenHangHoa = product.TenHangHoa,
                    SoLuongTon = 1,
                    DonVi = product.DonVi,
                    HinhAnh = product.HinhAnh,
                    MaLoai = product.MaLoai,
                    GiaBan = (decimal)product.GiaBan // Chuyển đổi kiểu int sang decimal

                };
                cart.AddItem(cartItem);
            }
            return RedirectToAction("Product_Index");
        }

    
        // Xóa Giỏ Hàng
        public ActionResult RemoveFromCart(string productId)
        {
            var cart = GetCart();
            cart.RemoveItem(productId); // Giả sử có phương thức RemoveItem trong ShoppingCart
            return RedirectToAction("XemGioHang"); // Chuyển hướng đến trang giỏ hàng hoặc trang khác
        }
        public ActionResult XemGioHang()
        {
            var cart = GetCart();
            return View(cart);
        }
        // Cap Nhat SL
        public ActionResult UpdateCart(string productId, int quantity)
        {
            var cart = GetCart();

            if (quantity <= 0)
            {
                cart.RemoveItem(productId);
            }
            else
            {
                cart.UpdateItemQuantity(productId, quantity); // Giả sử có phương thức UpdateItemQuantity trong ShoppingCart
            }

            return RedirectToAction("XemGioHang"); // Chuyển hướng đến trang giỏ hàng
        }
      

        public ActionResult XacNhanThongTin()
        {
            // Retrieve the customer from the session
            KhachHang kh = Session["Khachhang"] as KhachHang;

            // If customer is not logged in, redirect to login page
            if (kh == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }

            // Retrieve the shopping cart
            ShoppingCart cart = GetCart();
           

            // Check if the cart has items
            if (cart != null && cart.GetItems().Any()) // Ensure cart is not null and contains items
            {
                ViewBag.cart = cart.GetItems();
                return View(kh); // Pass the customer and cart to the view
            }
            else
            {
                ViewBag.Notification = "Your shopping cart is empty."; // Set notification for empty cart
                return View("EmptyCart"); // Redirect to EmptyCart view
            }
        }
        public ActionResult EmptyCart()
        {
            return View();
        }

        public string indexMaDBH()
        {
            var lastOrder = data.DonBanHangs.OrderByDescending(k => k.MaDonBanHang).FirstOrDefault();

            string newOrderId;

            if (lastOrder != null)
            {
                // Extract the numeric part from the last order ID
                string lastId = lastOrder.MaDonBanHang;
                int numericPart = int.Parse(lastId.Substring(3)); // Extracting the numeric part

                // Increment the numeric part
                int newNumericPart = numericPart + 1;

                // Format the new ID as "DBHXXX" where XXX is the incremented number with leading zeros if needed
                newOrderId = $"DBH{newNumericPart:D3}";
            }
            else
            {
                // Default to DBH001 if there are no existing orders
                newOrderId = "DBH001";
            }
            return newOrderId;
        }

            // Xử lý đặt hàng sau khi xác nhận
        [HttpPost]
        public ActionResult PlaceOrder(FormCollection col)
        {
            KhachHang kh = Session["Khachhang"] as KhachHang;
            ShoppingCart cart = GetCart();
            var giohang = cart.GetItems();
            if (!string.IsNullOrEmpty(col["txtDate"]))
            {
                DateTime ngaygiao;
                if (DateTime.TryParse(col["txtDate"], out ngaygiao))
                {
                    if (!string.IsNullOrEmpty(col["txtDatePay"]))
                    {
                        DateTime ngaythanhtoan;
                        if (DateTime.TryParse(col["txtDatePay"], out ngaythanhtoan))
                        {
                            DonBanHang hd = new DonBanHang();
                            hd.MaKH = kh.MaKH;
                            hd.NgayDat = DateTime.Now;
                            hd.NgayGiao = ngaygiao;
                            hd.NgayThanhToan = ngaythanhtoan;
                            hd.MaDonBanHang = indexMaDBH();
                            data.DonBanHangs.InsertOnSubmit(hd);
                            data.SubmitChanges();

                            foreach (var item in giohang)
                            {
                                ChiTietDonBanHang ct = new ChiTietDonBanHang();
                                ct.MaDonBanHang = hd.MaDonBanHang;
                                ct.MaHH = item.MaHH;
                                ct.SoLuong = item.SoLuongTon;
                                ct.DonGia = double.Parse(item.GiaBan.ToString());
                                data.ChiTietDonBanHangs.InsertOnSubmit(ct);

                                var hangHoa = data.HangHoas.FirstOrDefault(h => h.MaHH == ct.MaHH);

                                if (hangHoa != null)
                                {
                                    hangHoa.SoLuongTon -= ct.SoLuong;

                                    data.SubmitChanges();
                                }

                            }
                            data.SubmitChanges();
                            giohang.Clear();
                            ViewBag.p1 = "OrderConfirmation";
                            ViewBag.p = "Thank You for Your Order!";
                            ViewBag.tb = "Your order has been placed successfully. We will process it shortly.";
                            return View();
                        }
                        else
                        {
                            ViewBag.tb = "Ngày thanh toán không hợp lệ";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.tb = "Ngày thanh toán không được để trống";
                        return View();
                    }
                }
                else
                {
                    ViewBag.tb = "Ngày giao không hợp lệ";
                    return View();
                }
            }
            else
            {
                ViewBag.tb = "Ngày giao không được để trống";
                return View();
            }
        }
        
    

    // Hiển thị trang xác nhận đơn hàng
        
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapXLThem(FormCollection c)
        {
            string tk = c["txtTen"];
            string mk = c["txtMK"];
            bool isEmployee = c["isEmployee"] == "on";  // Check if the checkbox is checked

            if (isEmployee)
            {
                // Handle employee login
                NhanVien nv = data.NhanViens.SingleOrDefault(t => t.MaNV == tk && t.MatKhau == mk);
                if (nv == null)
                {
                    ViewBag.Notification = "Incorrect employee ID or password. Please try again.";
                    return RedirectToAction("DangNhap");
                }
                Session["nhanvien"] = nv;  // Set session for employee
                return RedirectToAction("TrangChu", "NhanVien");  // Redirect employees to NVController
            }
            else
            {
                // Handle customer login
                KhachHang kh = data.KhachHangs.SingleOrDefault(t => t.TaiKhoan == tk && t.MatKhau == mk);
                if (kh == null)
                {
                    ViewBag.Notification = "Incorrect username or password. Please try again.";
                    return RedirectToAction("DangNhap");
                }
                Session["khachhang"] = kh;  // Set session for customer
                return RedirectToAction("Index", "Home");  // Redirect customers to HomeController
            }
        }

        public ActionResult DangXuat()
        {
            Session["Khachhang"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKyXLThem(KhachHang kh)
        {
            // Assign the new ID to the customer's MaKH
            kh.MaKH = indexMaKH();
            data.KhachHangs.InsertOnSubmit(kh);
            data.SubmitChanges();
            Session["khachhang"] = kh;
            return RedirectToAction("Index", "Home");
        }
        public string indexMaKH()
        {
            var lastCustomer = data.KhachHangs.OrderByDescending(k => k.MaKH).FirstOrDefault();

            // Initialize new ID
            string newCustomerId;

            if (lastCustomer != null)
            {
                // Extract the numeric part from the last customer ID
                string lastId = lastCustomer.MaKH;
                int numericPart = int.Parse(lastId.Substring(2));

                // Increment the numeric part
                int newNumericPart = numericPart + 1;

                // Format the new ID as "KHXX" where XX is the incremented number with leading zeros if needed
                newCustomerId = $"KH{newNumericPart:D2}";
            }
            else
            {
                // Default to KH01 if there are no existing customers
                newCustomerId = "KH001";
            }
            return newCustomerId;
        }
        public ActionResult HistoryOrder(string id)
        {
           
            KhachHang kh = Session["Khachhang"] as KhachHang;
            if (kh == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            List<DonBanHang> dh =  data.DonBanHangs.Where(t => t.MaKH == id).ToList();
            ViewBag.dhang = dh;
            return View(kh);
        }






        public static IFirebaseClient CreateFirebaseClient()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "EZn5uE1hYD86b7U7Vd8PEeisvvOmOp0TSrYlecjG",
                BasePath = "https://danhgia-7fcb0-default-rtdb.asia-southeast1.firebasedatabase.app"
            };

            IFirebaseClient client;

            client = new FireSharp.FirebaseClient(config);
            return client;
        }
        public static async Task<DanhGia> FirebaseGetThongTinKhoa(IFirebaseClient client, string rootName)
        {

            if (client != null)
            {
                FirebaseResponse response = await client.GetAsync(rootName);


                return response.ResultAs<DanhGia>();
            }
            return null;
        }

        public static async Task<List<DanhGia>> GetThongTinKhoa(string rootName)
        {
            IFirebaseClient client = CreateFirebaseClient();

            List<DanhGia> ds = new List<DanhGia>();

            int dem = 2;
            int stt = 1;
            bool co = true;
            while (co)
            {
                try
                {
                    DanhGia tk = await FirebaseGetThongTinKhoa(client, rootName + "/" + stt.ToString() + "/");
                    if (tk == null)
                    {
                        co = false;
                        break;
                    }

                    stt++;
                    ds.Add(tk);
                }
                catch (Exception ex)
                {
                    co = false;
                }
            }
            return ds;

        }


        public static async Task FirebaseInsertData(IFirebaseClient client, object data, string rootName)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Firebase client cannot be null");
            }

            try
            {
                FirebaseResponse response = await client.SetAsync(rootName, data);
                //if (response.StatusCode != System.Net.HttpStatusCode.OK)
                //{
                //    throw new Exception("Error inserting data into Firebase");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting data: {ex.Message}");
                throw;
            }
        }

        public static async void FirebaseUpdateData(IFirebaseClient client, object data, string rootName)
        {
            if (client != null)
            {
                await client.UpdateAsync(rootName, data);
            }
        }
        public static async void FirebaseDeleteData(IFirebaseClient client, string rootName)
        {
            if (client != null)
            {
                await client.DeleteAsync(rootName);
            }
        }


    }

}
    
