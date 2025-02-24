using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class BatchResponseModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

        public int? Quantity { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime? ImportDate { get; set; }
        public DateTime? ManufactureDate { get; set; }

        public string Status { get; set; }
    }
}
