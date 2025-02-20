using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Voucher :BaseEntity
    {
        public int VoucherStorageID { get; set; }
        public double? DiscountPercentage { get; set; }
        public double? MinimumPurchase { get; set; }
        public int? Quantity { get; set; }
        public double? Value { get; set; }
        public bool? Status { get; set; }

        // Navigation properties
        public virtual VoucherStorage VoucherStorage { get; set; }

    }
}
