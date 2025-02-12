using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Brand : BaseEntity
    {
        public string BrandName { get; set; }

        public string? Description { get; set; }
        public string Origin { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
