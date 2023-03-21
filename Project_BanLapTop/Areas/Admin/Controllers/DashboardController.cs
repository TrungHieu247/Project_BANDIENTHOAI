using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_BanLapTop.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["userLogin"] == null || Session["userLogin"].ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            } 
            return View();
        }
    }
}