using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_BanLapTop.Controllers
{
    public class CartController : Controller
    {
        public ActionResult Index()
        {
            List<Cart> listCard = LibraryCart.GetCart();
            return View(listCard);
        }

        [HttpPost]
        public ActionResult AddCart(int id, string strUrl, FormCollection collection)
        {
            List<Cart> listCart = LibraryCart.GetCart();
            Cart c = listCart.Find(m => m.Id == id);
            var num_order = int.Parse(collection["num_order"]);
            if (c == null)
            {
                c = new Cart(id,num_order);
                listCart.Add(c);
                return Redirect(strUrl);
            }
            else
            {
                c.Quantity += num_order;
                return Redirect(strUrl);
            }
        }
        
        public ActionResult DeleteCart(int id)
        {
            List<Cart> listCart = LibraryCart.GetCart();
            Cart c = listCart.SingleOrDefault(m => m.Id == id);
            if (c != null)
            {
                listCart.Remove(c);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateCart(int id,int quantity)
        {
            List<Cart> listCart = LibraryCart.GetCart();
            Cart c = listCart.SingleOrDefault(m => m.Id == id);
            c.Quantity = quantity;
            var sub_total = c.Total;
            var total = LibraryCart.TotalPrice();
            var data = new {sub_total = string.Format("{0:0,0}", sub_total), total = string.Format("{0:0,0}", total) };  
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteAll()
        {
            List<Cart> listCart = LibraryCart.GetCart();
            listCart.Clear();
            return RedirectToAction("Index");
        }
    }
}