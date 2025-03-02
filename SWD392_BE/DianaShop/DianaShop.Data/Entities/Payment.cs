using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Payment : BaseEntity
    {
        public int UserID { get; set; }
        public int OrderID { get; set; }
        public int PaymentMethodID { get; set; }
        public string Address { get; set; }
        public int? VoucherID { get; set; }
        public decimal? FinalAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int StatusId { get; set; }

        public User User { get; set; }
        public Order Order { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Voucher Voucher { get; set; }
        public StageStatus StageStatus { get; set; }

    }
}
