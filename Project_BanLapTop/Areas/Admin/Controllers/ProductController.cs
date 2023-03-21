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

        public ActionResult Index()
        {
            return View(data.tb_products.ToList());
        }

        public ActionResult Create()
        {
            tb_product product = new tb_product();
            product.ListCategory = data.tb_categories.ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tb_product model,HttpPostedFileBase image,List<HttpPostedFileBase> detail_images)
        {
            if (!ModelState.IsValid)
                return View();
            if (image.ContentLength > 0)
            {
                string rootFolder = Server.MapPath("/Content/images/product/");
                string pathImage = rootFolder + image.FileName;
                image.SaveAs(pathImage);
                model.Image = "/Content/images/product/" + image.FileName;
            }
            //model.CreatedBy = GetFullName();
            model.CreatedDate = DateTime.Now;
            data.tb_products.InsertOnSubmit(model);
            data.SubmitChanges();
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

        public ActionResult Update(int id)
        {
            var product = data.tb_products.FirstOrDefault(m => m.Id == id);
            product.ListCategory = data.tb_categories.ToList();
            product.ListProductImage = data.tb_productImages.Where(m => m.Product_Id == id).ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(int id,tb_product model,HttpPostedFileBase image,List<HttpPostedFileBase> detail_images)
        {
            var product = data.tb_products.FirstOrDefault(m => m.Id == id);
            if (!ModelState.IsValid)
                return View();

            // Xử lý ảnh đại diện
            if(image != null)
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
            //model.CreatedBy = GetFullName();
            model.ModifiedDate = DateTime.Now;
            UpdateModel(model);
            data.SubmitChanges();

            // Cập nhật các ảnh chi tiết
            if(detail_images != null)
            {
                if (detail_images.Count > 0)
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

        public ActionResult Delete(int id)
        {
            var product = data.tb_products.FirstOrDefault(m => m.Id == id);
            // Xóa ảnh cũ
            string fullPath = Request.MapPath(product.Image);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);    
            }
            data.tb_products.DeleteOnSubmit(product);
            data.SubmitChanges();
            return RedirectToAction("Index", "Product");
        }

    }
}