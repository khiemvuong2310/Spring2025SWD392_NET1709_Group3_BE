using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class CartResponseModel
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public IEnumerable<ProductResponseModel> Products { get; set; }
        public int Quantity { get; set; }
    }
}
