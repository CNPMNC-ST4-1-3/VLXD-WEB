using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_BMS.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public List<CardItems> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }
}