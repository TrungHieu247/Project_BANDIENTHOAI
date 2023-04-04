using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Project_BanLapTop.Controllers
{
    public class AccountController : Controller
    {
        private MydataDataContext data = new MydataDataContext();
        public ActionResult Login()
        {
            if (Session["Account"] != null)
                return RedirectToAction("Index", "Home");
            else
                return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection,string beforeUrl)
        {
            var username = collection["username"];
            var password = collection["password"];
            var remember_me = collection["remember_me"];
            tb_guest guest = data.tb_guests.SingleOrDefault(n => n.Username == username && n.Password == password);
            if (guest == null)
            {
                TempData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
            else
            {
                if (remember_me == "true")
                {
                    HttpCookie usernameCookie = new HttpCookie("usernameGuestCookie", username);
                    usernameCookie.Expires = DateTime.Now.AddHours(1);
                    Response.Cookies.Add(usernameCookie);
                }
                TempData["Success"] = "Đăng nhập thành công";
                Session["Account"] = guest;
                if(beforeUrl == "https://localhost:44370/Account/Register")
                {
                    beforeUrl = "/";
                }
                return Redirect(beforeUrl);
            }
        }

        public ActionResult Register()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Register(tb_guest model,FormCollection collection)
        {
            var confirm_password = collection["confirm_password"];
            if (string.IsNullOrEmpty(confirm_password))
            {
                TempData["Confirm_password"] = "Vui lòng nhập mật khẩu xác nhận";
            }
            else
            {
                if (!model.Password.Equals(confirm_password))
                {
                    TempData["Password"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";
                }
                else
                {
                    if (LibraryCart.CheckUser(model.Username, model.Email) == true)
                    {
                        TempData["UserExsist"] = "Tên đăng nhập hoặc email đã tồn tại";
                        return View();
                    }
                    data.tb_guests.InsertOnSubmit(model);
                    data.SubmitChanges();
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        public ActionResult Logout(string currentUrl)
        {
            // Hủy cookie remember và username
            if (Request.Cookies["usernameGuestCookie"] != null)
            {
                var infock = new HttpCookie("usernameGuestCookie");
                infock.Expires = DateTime.Now.AddHours(-1);
                Response.Cookies.Add(infock);
            }
            // Xóa session userlogin
            Session.Remove("Account");
            FormsAuthentication.SignOut();
            return Redirect(currentUrl);
        }
    }
}