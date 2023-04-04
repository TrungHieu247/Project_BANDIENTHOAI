using Newtonsoft.Json.Linq;
using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Project_BanLapTop.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private MydataDataContext data = new MydataDataContext();

        public ActionResult Login()
        {
            if(Request.Cookies.Get("usernameCookie") != null)
            {
                var username = Request.Cookies.Get("usernameCookie").Value;
                var user = data.tb_users.FirstOrDefault(m => m.Username == username);
                Session["userLogin"] = user;
            }
            if (Session["userLogin"] != null)
                return Redirect("~/Admin");
            else
                return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var username = collection["username"];  
            var password = collection["password"];
            var remember_me = collection["remember_me"];
            tb_user user_login = data.tb_users.FirstOrDefault(m => m.Username == username && m.Password == password);
            if(user_login != null)
            {
                // Kiểm tra ghi nhớ đăng nhập
                if(remember_me == "true")
                {
                    HttpCookie usernameCookie = new HttpCookie("usernameCookie", username);
                    usernameCookie.Expires = DateTime.Now.AddHours(1);
                    Response.Cookies.Add(usernameCookie);
                }
                TempData["Success"] = "Đăng nhập thành công";
                Session["userLogin"] = user_login;
                return Redirect("~/Admin");
            }
            else
            {
                TempData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
        }
        public ActionResult Logout()
        {
            // Hủy cookie remember và username
            if (Request.Cookies["usernameCookie"] != null)
            {
                var infock = new HttpCookie("usernameCookie");
                infock.Expires = DateTime.Now.AddHours(-1);
                Response.Cookies.Add(infock);
            }
            // Xóa session userlogin
            Session.Remove("userLogin");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}