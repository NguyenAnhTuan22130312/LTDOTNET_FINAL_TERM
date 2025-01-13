using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.ViewModel
{
    public class ProductOrder

    {
        public int IdFood { get; set; } // ID sản phẩm
        public string FoodName { get; set; } // Tên sản phẩm
        public int? Price { get; set; } // Giá sản phẩm (có thể null)
        public string Img { get; set; } // URL ảnh sản phẩm (có thể null)
        public int? Quantity { get; set; } // Số lượng sản phẩm (có thể null)
    }

}
