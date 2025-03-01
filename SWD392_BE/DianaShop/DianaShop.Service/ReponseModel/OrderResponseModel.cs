using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class OrderResponseModel
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? UpdateDate { get; set; }
        public List<OrderDetailResponseModel> Details { get; set; }
    }
}
