using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Ast.Selectors;

namespace Project_BanLapTop.Controllers
{
    public class CheckoutController : Controller
    {
        private MydataDataContext data = new MydataDataContext();
        public ActionResult Index()
        {
            if (Session["Account"] == null || Session["Account"].ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }
            var list_cart = LibraryCart.GetCart();
            if (list_cart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(list_cart);
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            tb_order order = new tb_order();
            tb_guest guest = (tb_guest)Session["Account"];
            tb_product product = new tb_product();
            List<Cart> list_cart = LibraryCart.GetCart();

            // Thêm đơn hàng
            order.GuestID = guest.Id;
            order.CreatedDate = DateTime.Now;
            order.Status = "processing";
            order.Quantity = LibraryCart.TotalQuantity();
            order.Total = (decimal)LibraryCart.TotalPrice();
            order.Note = collection["note"];
            data.tb_orders.InsertOnSubmit(order);
            data.SubmitChanges();

            // Thêm chi tiết đơn hàng
            foreach(var cart in list_cart)
            {
                tb_orderDetail orderDetail = new tb_orderDetail();
                orderDetail.ProductID = cart.Id;
                orderDetail.OrderId = order.Id;
                orderDetail.Quantity = cart.Quantity;
                orderDetail.Price = (decimal)cart.Price;

                // Cập nhật số lượng tồn sản phẩm
                product = data.tb_products.SingleOrDefault(p => p.Id == cart.Id);
                product.QuantityInSock -= cart.Quantity;
                data.SubmitChanges();
                data.tb_orderDetails.InsertOnSubmit(orderDetail);
            }
            data.SubmitChanges();
            return RedirectToAction("CheckoutSuccess");
        }

        public ActionResult CheckoutSuccess()
        {
            var list_cart = LibraryCart.GetCart();
            Session["Cart"] = null;
            return View(list_cart);
        }
    }
}