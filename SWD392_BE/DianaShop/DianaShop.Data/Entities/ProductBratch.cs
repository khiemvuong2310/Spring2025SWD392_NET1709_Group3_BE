using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class ProductBratch : BaseEntity
    {
        public int ProductID { get; set; }

        public int? Quantity { get; set; }

        public DateTime? ExpDate { get; set; }

        public DateTime? ImportDate { get; set; }

        public bool? Status { get; set; } // true: active, false: expired

        public virtual Product Products { get; set; }
    }
}
