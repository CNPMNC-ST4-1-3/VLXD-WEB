using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_BMS.Models
{
    public class ChiTietDonHangViewModel
    {
        public string MaHH { get; set; }
        public string TenHangHoa { get; set; }
        public int? SoLuong { get; set; }
        public double? DonGia { get; set; }
    }

    public class DonHangViewModel
    {
        public DonBanHang DonHang { get; set; }
        public double TongTien { get; set; }
        public List<ChiTietDonHangViewModel> ChiTiet { get; set; } // Danh sách chi tiết
    }
}