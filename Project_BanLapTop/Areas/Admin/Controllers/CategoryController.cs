using PagedList;
using Project_BanLapTop.App_Start;
using Project_BanLapTop.Areas.Admin.Data;
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
        private MydataDataContext data = new MydataDataContext();

        [AdminAuthorize(idRole = "List")]
        public ActionResult Index(int? page,int? pageSize)
        {
            if (page == null) page = 1;
            if(pageSize == null) pageSize = 5;
            var list_category = data.tb_categories.ToList();
            ViewBag.PageSize = pageSize;
            return View(list_category.ToPagedList((int)page, (int)pageSize));
        }

        [HttpPost]
        public ActionResult Create(tb_category model)
        {
            if (!ModelState.IsValid)
            {
                TempData["RequiredName"] = "Không được bỏ trống tên danh mục";
                return RedirectToAction("Index");
            }
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = MyLibrary.GetFullnameByUserLogin();
            data.tb_categories.InsertOnSubmit(model);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }


        [AdminAuthorize(idRole = "Update")]
        public ActionResult Update(int id, int? page, int? pageSize)
        {
            var category = data.tb_categories.FirstOrDefault(m => m.Id == id);
            var list_category = data.tb_categories.ToList();
            if (page == null) page = 1;
            if (pageSize == null) pageSize = 5;
            ViewBag.PageSize = pageSize;
            var dataModel = new Tuple<tb_category, IPagedList<tb_category>>(category, list_category.ToPagedList((int)page, (int)pageSize));
            return View(dataModel);
        }

        [HttpPost]
        public ActionResult Update(int id, tb_category model)
        {
            if (!ModelState.IsValid)
            {
                TempData["RequiredName"] = "Không được bỏ trống tên danh mục";
                return RedirectToAction("Update");
            }
            var category = data.tb_categories.FirstOrDefault(m => m.Id == id);
            category.Name = model.Name;
            category.ModifiedDate = DateTime.Now;
            category.ModifiedBy = MyLibrary.GetFullnameByUserLogin();
            UpdateModel(category);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }

        [AdminAuthorize(idRole = "Delete")]
        public ActionResult Delete(int id)
        {
            var category = data.tb_categories.FirstOrDefault(m => m.Id == id);
            data.tb_categories.DeleteOnSubmit(category);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}