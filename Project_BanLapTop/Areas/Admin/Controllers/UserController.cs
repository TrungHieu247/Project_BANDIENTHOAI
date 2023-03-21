using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_BanLapTop.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        public string GetFullName()
        {
            var userLogin = (tb_user)Session["UserLogin"];
            return userLogin.Fullname;
        }

        MydataDataContext data = new MydataDataContext();
        public ActionResult Index()
        {
            var listuser = data.tb_users.ToList();
            return View(listuser);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tb_user model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var fullname = GetFullName();
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = fullname;
            data.tb_users.InsertOnSubmit(model);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Update(int id)
        {
            var user = data.tb_users.FirstOrDefault(m => m.Id == id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, tb_user model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var fullname = GetFullName();
            var user = data.tb_users.FirstOrDefault(m => m.Id == id);
            user.Fullname = model.Fullname;
            user.Password = model.Password;
            user.ModifiledDate = DateTime.Now;
            user.ModifiledBy = fullname;
            UpdateModel(user);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var user = data.tb_users.FirstOrDefault(m => m.Id == id);
            data.tb_users.DeleteOnSubmit(user);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
       
    }
}