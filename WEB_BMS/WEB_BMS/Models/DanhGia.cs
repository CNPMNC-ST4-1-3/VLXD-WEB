using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_BMS.Models
{
    public class DanhGia
    {
        public List<DanhGia> Reviews { get; set; }
        public string HoTen { get; set; }
        public string BinhLuan { get; set; }
    }
}