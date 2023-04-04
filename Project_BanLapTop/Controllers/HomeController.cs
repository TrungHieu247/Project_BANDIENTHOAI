using PagedList;
using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_BanLapTop.Controllers
{
    public class HomeController : Controller
    {
        private MydataDataContext data = new MydataDataContext();
        public ActionResult Index()
        {
            if (Request.Cookies.Get("usernameGuestCookie") != null)
            {
                var username = Request.Cookies.Get("usernameGuestCookie").Value;
                var guest = data.tb_guests.FirstOrDefault(m => m.Username == username);
                Session["Account"] = guest;
            }
            var products = data.tb_products.ToList();
            var categories = data.tb_categories.ToList();
            var homeViewModel = new Tuple<List<tb_category>, List<tb_product>>(categories,products);
            return View(homeViewModel);
        }

        public ActionResult ProductCategory(int id,int? page,int? pageSize)
        {
            if (page == null) page = 1;
            if (pageSize == null) pageSize = 3;
            var categories = data.tb_categories.ToList();
            var list_product_cate = data.tb_products.Where(m => m.Category_Id == id).ToList();
            var viewModel = new Tuple<List<tb_category>, IPagedList<tb_product>>(categories, list_product_cate.ToPagedList((int)page, (int)pageSize));
            ViewBag.Cate_ID = id;
            ViewBag.PageSize = pageSize;
            return View(viewModel);
        }

        public ActionResult ProductDetail(int id,string returnUrl)
        {
            var categories = data.tb_categories.ToList();    
            var product = data.tb_products.FirstOrDefault(m => m.Id == id);
            var list_product_image = data.tb_productImages.Where(m => m.Product_Id == id).ToList();
            var list_product_category = data.tb_products.Where(m => m.Category_Id == product.Category_Id).ToList();
            var viewModel = new Tuple<List<tb_category>, tb_product, List<tb_productImage>, List<tb_product>>(categories,product, list_product_image,list_product_category);
            return View(viewModel);
        }

        public ActionResult SearchProduct(string key)
        {
            var categories = data.tb_categories.ToList();
            var list_product_search = data.tb_products.Where(m => m.Name.Contains(key)).ToList();
            ViewBag.Key = key;
            var viewModel = new Tuple<List<tb_category>, List<tb_product>>(categories, list_product_search);
            return View(viewModel);
        }
    }
}