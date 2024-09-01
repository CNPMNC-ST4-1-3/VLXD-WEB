using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_BMS.Models
{
    public class CardItems
    {
        public string MaHH { get; set; }
        public string TenHangHoa { get; set; }
        public string DonVi { get; set; }
        public int SoLuongTon { get; set; }
        public string HinhAnh { get; set; }
        public string MaLoai { get; set; }
        public decimal GiaBan { get; set; }
        public decimal Total => SoLuongTon * GiaBan;
    }

    public class ShoppingCart
    {
        public List<CardItems> Items { get; set; } = new List<CardItems>();
        public List<CardItems> GetItems()
        {
            return Items;
        }

        public void AddItem(CardItems item)
        {
            var existingItem = Items.FirstOrDefault(i => i.MaHH == item.MaHH);
            if (existingItem != null)
            {
                existingItem.SoLuongTon += item.SoLuongTon;
            }
            else
            {
                Items.Add(item);
            }
        }
        public int GetItemCount()
        {

            if (Items != null)
            {
                return Items.Count; // Returns the number of items in the list
            }
            return 0;
        }
            public void RemoveItem(string productId)
            {
            var item = Items.FirstOrDefault(i => i.MaHH == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public decimal GetTotal()
        {
            return Items.Sum(i => i.Total);
        }
        public void UpdateItemQuantity(string productId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.MaHH== productId);
            if (item != null)
            {
                item.SoLuongTon = quantity;
            }
        }
    }
}