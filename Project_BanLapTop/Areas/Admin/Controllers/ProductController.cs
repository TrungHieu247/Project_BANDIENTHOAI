using PagedList;
using Project_BanLapTop.App_Start;
using Project_BanLapTop.Areas.Admin.Data;
using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_BanLapTop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private MydataDataContext data = new MydataDataContext();

        [AdminAuthorize(idRole = "List")]
        public ActionResult Index(int? page,int? pageSize,string key = "")
        {
            if (page == null)
                page = 1;
            if (pageSize == null)
                pageSize = 3;
            ViewBag.Key = key;
            ViewBag.PageSize = pageSize;    
            return View(data.tb_products.Where(m => m.Name.Contains(key)).ToList().ToPagedList((int)page,(int)pageSize));
        }

        [AdminAuthorize(idRole = "Create")]
        public ActionResult Create()
        {
            var product = new tb_product();
            var list_category = data.tb_categories.ToList();
            var dataModel = new Tuple<tb_product, List<tb_category>>(product, list_category);
            return View(dataModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tb_product model,HttpPostedFileBase image,List<HttpPostedFileBase> detail_images)
        {
            if (!ModelState.IsValid)
            {
                var list_category = data.tb_categories.ToList();
                var dataModel = new Tuple<tb_product, List<tb_category>>(model, list_category);
                return View(dataModel);
            }
            if (image.ContentLength > 0)
            {
                string rootFolder = Server.MapPath("/Content/images/product/");
                string pathImage = rootFolder + image.FileName;
                image.SaveAs(pathImage);
                model.Image = "/Content/images/product/" + image.FileName;
            }
            model.CreatedBy = MyLibrary.GetFullnameByUserLogin();
            model.CreatedDate = DateTime.Now;
            data.tb_products.InsertOnSubmit(model);
            data.SubmitChanges();
            TempData["Status"] = "Thêm sản phẩm mới thành công";
            int id = model.Id;

            // Xử lý ảnh chi tiết
            if(detail_images.Count > 0)
            {
                foreach (var item in detail_images)
                {
                    if (item.ContentLength > 0)
                    {
                        // Lưu các hình vào server
                        string rootFolder = Server.MapPath("/Content/images/product/");
                        string pathImage = rootFolder + item.FileName;
                        item.SaveAs(pathImage);
                        // Lưu các ảnh vào database
                        tb_productImage productImage = new tb_productImage();
                        productImage.Image = "/Content/images/product/" + item.FileName;
                        productImage.Product_Id = id;
                        data.tb_productImages.InsertOnSubmit(productImage);
                        data.SubmitChanges();
                    }
                }
            }
            return RedirectToAction("Index","Product");
        }


        [AdminAuthorize(idRole = "Update")]
        public ActionResult Update(int id)
        {
            var product = data.tb_products.FirstOrDefault(m => m.Id == id);
            var list_category = data.tb_categories.ToList();
            var list_product_image = data.tb_productImages.Where(m => m.Product_Id == id).ToList();
            var dataModel = new Tuple<tb_product, List<tb_category>, List<tb_productImage>>(product,list_category,list_product_image);
            return View(dataModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(int id,tb_product model,HttpPostedFileBase image,List<HttpPostedFileBase> detail_images)
        {
            var product = data.tb_products.FirstOrDefault(m => m.Id == id);
            if (!ModelState.IsValid)
            {
                var list_category = data.tb_categories.ToList();
                var list_product_image = data.tb_productImages.Where(m => m.Product_Id == id).ToList();
                var dataModel = new Tuple<tb_product, List<tb_category>, List<tb_productImage>>(product, list_category, list_product_image);
                return View(dataModel);
            }

            // Xử lý ảnh đại diện
            if (image != null)
            {
                if(image.ContentLength > 0)
                {
                    // Xóa ảnh cũ
                    string fullPath = Request.MapPath(product.Image);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    // Cập nhật ảnh khác
                    string rootFolder = Server.MapPath("/Content/images/product/");
                    string pathImage = rootFolder + image.FileName;
                    image.SaveAs(pathImage);
                    product.Image = "/Content/images/product/" + image.FileName;
                }
            }
            // Xử lý cập nhật dữ liệu 
            product.Name = model.Name;
            product.ProductCode = model.ProductCode;
            product.Price = model.Price;
            product.QuantityInSock = model.QuantityInSock;
            product.Category_Id = model.Category_Id;
            product.DetailDescription = model.DetailDescription;
            product.ShortDescription = model.ShortDescription;
            product.SellWell = model.SellWell == true ? true: false;
            product.Outstanding = model.Outstanding == true ? true : false;
            //model.ModifiedBy = GetFullName();
            model.ModifiedDate = DateTime.Now;
            UpdateModel(model);
            data.SubmitChanges();
            TempData["Status"] = "Cập nhật dữ liệu sản phẩm thành công";

            // Cập nhật các ảnh chi tiết
            if (detail_images[0] != null)
            {
                    // Xóa các ảnh chi tiết
                    var list_images = data.tb_productImages.Where(m => m.Product_Id == id).ToList();
                    foreach (var item in list_images)
                    {
                        string fullPath = Request.MapPath(item.Image);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    data.tb_productImages.DeleteAllOnSubmit(list_images);
                    data.SubmitChanges();

                    // Thêm các ảnh chi tiết mới
                    foreach (var item in detail_images)
                    {
                        if(item != null)
                        {
                            if (item.ContentLength > 0)
                            {
                                // Lưu các hình vào server
                                string rootFolder = Server.MapPath("/Content/images/product/");
                                string pathImage = rootFolder + item.FileName;
                                item.SaveAs(pathImage);
                                // Lưu các ảnh vào database
                                tb_productImage productImage = new tb_productImage();
                                productImage.Image = "/Content/images/product/" + item.FileName;
                                productImage.Product_Id = id;
                                data.tb_productImages.InsertOnSubmit(productImage);
                                data.SubmitChanges();
                            }
                        }
                }
            }
            return RedirectToAction("Index", "Product");
        }


        [AdminAuthorize(idRole = "Delete")]
        public ActionResult Delete(int id)
        {
            // Xóa ảnh chi tiết
            var list_images = data.tb_productImages.Where(m => m.Product_Id == id).ToList();
            foreach (var item in list_images)
            {
                string fullPath1 = Request.MapPath(item.Image);
                if (System.IO.File.Exists(fullPath1))
                {
                    System.IO.File.Delete(fullPath1);
                }
            }

            data.tb_productImages.DeleteAllOnSubmit(list_images);
            data.SubmitChanges();

            var product = data.tb_products.FirstOrDefault(m => m.Id == id);
            // Xóa ảnh cũ
            string fullPath = Request.MapPath(product.Image);
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);    
            data.tb_products.DeleteOnSubmit(product);
            data.SubmitChanges();
            return RedirectToAction("Index", "Product");
        }

    }
}