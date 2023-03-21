using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_BanLapTop.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        public string GetFullName()
        {
            var userLogin = (tb_user)Session["UserLogin"];
            return userLogin.Fullname;
        }

        private MydataDataContext data = new MydataDataContext();
        public ActionResult Index()
        {
            var list_category = data.tb_categories.ToList();
            return View(list_category);
        }

        [HttpPost]
        public ActionResult Create(tb_category model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                data.tb_categories.InsertOnSubmit(model);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Update(int id)
        {
            var category = data.tb_categories.FirstOrDefault(m => m.Id == id);
            return View(category);
        }

        [HttpPost]
        public ActionResult Update(int id, tb_category model)
        {
            var category = data.tb_categories.FirstOrDefault(m => m.Id == id);
            category.Name = model.Name;
            //user.ModifiledBy = model.ModifiledBy;
            category.ModifiedDate = DateTime.Now;
            UpdateModel(category);
            data.SubmitChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var category = data.tb_categories.FirstOrDefault(m => m.Id == id);
            data.tb_categories.DeleteOnSubmit(category);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}