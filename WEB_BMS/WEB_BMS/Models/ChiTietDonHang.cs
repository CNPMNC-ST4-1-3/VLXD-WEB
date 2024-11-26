using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_BMS.Models
{
    public class ChiTietDonHang
    {
        public string MaDonBanHang { get; set; }
        public string TenHangHoa { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien { get; set; }
    }
}