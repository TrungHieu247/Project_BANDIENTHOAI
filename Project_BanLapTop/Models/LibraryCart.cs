using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_BanLapTop.Models
{
    public static class LibraryCart
    {
        private static MydataDataContext data = new MydataDataContext();
        public static List<Cart> GetCart()
        {
            List<Cart> list = HttpContext.Current.Session["Cart"] as List<Cart>;
            if (list == null)
            {
                list = new List<Cart>();
                HttpContext.Current.Session["Cart"] = list;
            }
            return list;
        }

        public static int TotalQuantity()
        {
            int s = 0;
            List<Cart> listCart = HttpContext.Current.Session["Cart"] as List<Cart>;
            if (listCart != null)
                s = listCart.Sum(m => m.Quantity);
            return s;
        }

        public static double TotalPrice()
        {
            double tt = 0;
            List<Cart> listcart = HttpContext.Current.Session["Cart"] as List<Cart>;
            if (listcart != null)
                tt = listcart.Sum(m => m.Total);
            return tt;
        }

        public static string GetFullNameLogin()
        {
            var guest = HttpContext.Current.Session["Account"] as tb_guest;
            return guest.Fullname;
        }

        public static bool CheckUser(string username, string email)
        {
            var check = data.tb_guests.Any(m => m.Username == username || m.Email == email);
            if (check == true)
            {
                return true;
            }
            return false;
        }
    }
}