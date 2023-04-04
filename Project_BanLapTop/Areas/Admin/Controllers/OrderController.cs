using PagedList;
using Project_BanLapTop.App_Start;
using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_BanLapTop.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        // GET: Admin/Order
        private MydataDataContext data = new MydataDataContext();

        [AdminAuthorize(idRole = "List")]
        public ActionResult Index(int? page, int? pageSize,string key  = "")
        {
            if (page == null) page = 1;
            if (pageSize == null) pageSize = 5;
            ViewBag.PageSize = pageSize;
            ViewBag.Key = key;
            var list_order = data.tb_orders.Where(m => m.tb_guest.Fullname.Contains(key)).ToList();
            return View(list_order.ToPagedList((int)page, (int)pageSize));
        }

        [AdminAuthorize(idRole = "Update")]
        public ActionResult Update(int id)
        {
            var order = data.tb_orders.SingleOrDefault(m => m.Id == id);
            var order_detail = data.tb_orderDetails.Where(m => m.OrderId == id).ToList();
            var dataModel = new Tuple<tb_order, List<tb_orderDetail>>(order, order_detail);
            return View(dataModel);
        }

        [HttpPost]
        public ActionResult Update(int id,FormCollection collection)
        {
            string status = collection["status"];
            tb_order order = data.tb_orders.SingleOrDefault(m => m.Id == id);
            order.Status = status;
            data.SubmitChanges();
            TempData["Status"] = "Cập nhật thông tin đơn hàng thành công";
            return RedirectToAction("Index");
        }
    }
}