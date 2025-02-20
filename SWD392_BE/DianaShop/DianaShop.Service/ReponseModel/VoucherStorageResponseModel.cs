using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class VoucherStorageResponseModel
    {
        public int Id { get; set; }
        public int? UserID { get; set; }
        public List<VoucherResponseModel> Vouchers { get; set; }
    }
}
