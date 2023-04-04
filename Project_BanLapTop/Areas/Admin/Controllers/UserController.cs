using PagedList;
using Project_BanLapTop.App_Start;
using Project_BanLapTop.Areas.Admin.Data;
using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Project_BanLapTop.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        MydataDataContext data = new MydataDataContext();

        [AdminAuthorize(idRole = "List")]
        public ActionResult Index(int? page, int? pageSize,string key = "")
        {
            if (page == null) page = 1;
            if (pageSize == null) pageSize = 4;
            ViewBag.PageSize = pageSize;
            ViewBag.Key = key;
            var listuser = data.tb_users.Where(m => m.Fullname.Contains(key)).ToList();
            return View(listuser.ToPagedList((int)page,(int)pageSize));
        }

        [AdminAuthorize(idRole = "Create")]
        public ActionResult Create()
        {
            tb_user user = new tb_user();
            ViewBag.List_role = data.tb_roles.ToList();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tb_user model, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string confirm_password = collection["Confirm_password"];
            if (string.IsNullOrEmpty(confirm_password))
            {
                TempData["Confirm_password"] = "Vui lòng nhập mật khẩu xác nhận";
                return View(model);
            }
            else
            {
                if (!model.Password.Equals(confirm_password))
                {
                    TempData["Password"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";
                }
                else
                {
                    if (MyLibrary.CheckUserExists(model.Username, model.Email) == true)
                    {
                        TempData["UserExsist"] = "Tên đăng nhập hoặc email đã tồn tại";
                        return View(model);
                    }
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = MyLibrary.GetFullnameByUserLogin();
                    data.tb_users.InsertOnSubmit(model);
                    data.SubmitChanges();
                    TempData["Status"] = "Thêm người dùng thành công";

                    // Thêm quyền cho user
                    var create = collection["Create"];
                    if(!string.IsNullOrEmpty(create))
                    {
                        tb_user_role user_role = new tb_user_role();
                        user_role.idRole = create;
                        user_role.idUser = model.Id;
                        data.tb_user_roles.InsertOnSubmit(user_role);
                        data.SubmitChanges();
                    }
                    var update = collection["Update"];
                    if (!string.IsNullOrEmpty(update))
                    {
                        tb_user_role user_role = new tb_user_role();
                        user_role.idRole = update;
                        user_role.idUser = model.Id;
                        data.tb_user_roles.InsertOnSubmit(user_role);
                        data.SubmitChanges();
                    }
                    var delete = collection["Delete"];
                    if (!string.IsNullOrEmpty(delete))
                    {
                        tb_user_role user_role = new tb_user_role();
                        user_role.idRole = delete;
                        user_role.idUser = model.Id;
                        data.tb_user_roles.InsertOnSubmit(user_role);
                        data.SubmitChanges();
                    }
                    var list = collection["List"];
                    if (!string.IsNullOrEmpty(list))
                    {
                        tb_user_role user_role = new tb_user_role();
                        user_role.idRole = list;
                        user_role.idUser = model.Id;
                        data.tb_user_roles.InsertOnSubmit(user_role);
                        data.SubmitChanges();
                    }
                    return RedirectToAction("Index");
                }
            }
            return View();
        }


        [AdminAuthorize(idRole = "Update")]
        public ActionResult Update(int id)
        {
            var user = data.tb_users.FirstOrDefault(m => m.Id == id);
            ViewBag.List_user_role = data.tb_user_roles.Where(m => m.idUser == id).ToList();
            ViewBag.List_role = data.tb_roles.ToList();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, tb_user model,FormCollection collection)
        {
            var user = data.tb_users.FirstOrDefault(m => m.Id == id);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            user.Fullname = model.Fullname;
            user.Email = model.Email;
            user.Password = model.Password;
            user.ModifiedDate = DateTime.Now;
            user.ModifiedBy = MyLibrary.GetFullnameByUserLogin();
            UpdateModel(user);
            data.SubmitChanges();
            TempData["Status"] = "Cập nhật thông tin người dùng thành công";
            return RedirectToAction("Index");
        }


        [AdminAuthorize(idRole = "Delete")]
        public ActionResult Delete(int id)
        {
            var user = data.tb_users.FirstOrDefault(m => m.Id == id);
            data.tb_users.DeleteOnSubmit(user);
            data.SubmitChanges();
            TempData["Status"] = "Xóa người dùng thành công";
            return RedirectToAction("Index");
        }
       
    }
}