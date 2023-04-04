using PagedList;
using Project_BanLapTop.App_Start;
using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Project_BanLapTop.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        private MydataDataContext data = new MydataDataContext();

        [AdminAuthorize(idRole = "List")]
        public ActionResult Index(int? page, int? pageSize)
        {
            if (page == null) page = 1;
            if (pageSize == null) pageSize = 5;
            ViewBag.PageSize = pageSize;
            ViewBag.Order_success = data.tb_orders.Count(m => m.Status == "success");
            ViewBag.Order_processing = data.tb_orders.Count(m => m.Status == "processing");
            ViewBag.Order_canceled = data.tb_orders.Count(m => m.Status == "canceled");
            ViewBag.Order_transported = data.tb_orders.Count(m => m.Status == "being_transported");
            ViewBag.Sales = data.tb_orders.Sum(m => m.Total);
            var list_order = data.tb_orders.ToList().OrderByDescending(m => m.CreatedDate);
            return View(list_order.ToPagedList((int)page, (int)pageSize));
        }

    }
}