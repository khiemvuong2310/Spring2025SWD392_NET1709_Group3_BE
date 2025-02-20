using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class VoucherStorageRequestModel
    {
        public int UserID { get; set; }
    }

    public class AddVoucherStorageRequestModel
    {
        public int UserID { get; set; }
        public int VoucherID { get; set; }
    }
}
