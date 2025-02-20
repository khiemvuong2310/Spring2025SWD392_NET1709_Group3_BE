using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class StorageResponseModel
    {
        public int VoucherId { get; set; }
        public int VoucherStorageId { get; set; }
        public int? Quantity { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
