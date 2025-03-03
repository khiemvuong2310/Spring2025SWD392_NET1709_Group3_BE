using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class StorageRequestModel
    {
        public int VoucherId { get; set; }
        public int VoucherStorageId { get; set; }
        //public int? Quantity { get; set; }
        public bool Status { get; set; }
    }
}
