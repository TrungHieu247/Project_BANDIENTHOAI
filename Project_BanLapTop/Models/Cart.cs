using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Project_BanLapTop.Models
{
    public class Cart
    {
        MydataDataContext data = new MydataDataContext();
        public int Id { get; set; }

        [DisplayName("Mã sản phẩm")]
        public string ProductCode { get; set; }

        [DisplayName("Tên sản phẩm")]
        public string Name { get; set; }

        [DisplayName("Hình ảnh")]
        public string Image { get; set; }

        [DisplayName("Giá bán")]
        public double Price { get; set; }

        [DisplayName("Số lượng")]
        public int Quantity { get; set; }

        [DisplayName("Thành tiền")]
        public double Total
        {
            get { return Price * Quantity; }
        }

        public Cart(int id,int num_order)
        {
            this.Id = id;
            tb_product s = data.tb_products.SingleOrDefault(m => m.Id == id);
            this.ProductCode = s.ProductCode;   
            this.Name = s.Name;
            this.Image = s.Image;
            this.Price = double.Parse(s.Price.ToString());
            this.Quantity = num_order;
        }
    }
}