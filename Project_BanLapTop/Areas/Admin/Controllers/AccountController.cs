using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_BanLapTop.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private MydataDataContext data = new MydataDataContext();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var username = collection["username"];  
            var password = collection["password"];  
            tb_user user_login = data.tb_users.FirstOrDefault(m => m.Username == username && m.Password == password);
            if(user_login != null)
            {
                // Sau khi đăng nhập thành công thì xuất thông báo success lên 
                TempData["Success"] = "Đăng nhập thành công";
                Session["userLogin"] = user_login;
            }
            else
            {
                TempData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
            return RedirectToAction("Index", "Dashboard");
        }

    }
}