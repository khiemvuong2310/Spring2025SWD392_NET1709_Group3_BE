using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Storage
    {
        public int VoucherId { get; set; }
        public int VoucherStorageId { get; set; }
        public int? Quantity { get; set; }

        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Voucher Voucher { get; set; }
        public virtual VoucherStorage VoucherStorage { get; set; }
    }
}
