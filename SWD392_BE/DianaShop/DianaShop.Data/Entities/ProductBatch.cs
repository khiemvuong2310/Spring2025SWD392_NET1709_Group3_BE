using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class ProductBatch : BaseEntity
    {
        public int ProductID { get; set; }

        public int? Quantity { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime? ImportDate { get; set; }
        public DateTime? ManufactureDate { get; set; }

        public bool? Status { get; set; } // true: active, false: expired

        public virtual Product Product { get; set; }
    }
}
