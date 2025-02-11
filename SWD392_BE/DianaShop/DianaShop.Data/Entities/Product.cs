using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace DianaShop.Data.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public int BrandID { get; set; }

        public int CategoryID { get; set; }

        public decimal? Price { get; set; }

        public string? ImageURL { get; set; }

        public string? Ingredients { get; set; }

        public string? Size { get; set; }

        public string? PackagingType { get; set; }

        public string? FormulationType { get; set; }

        public string? SNF { get; set; } // Có thể là "Skin Nutrition Factor"

        public bool Status { get; set; } // true: available, false: out of stock

        public virtual ICollection<Category> Categories { get; set; }

        public virtual Brand Brands { get; set; }

        public virtual ICollection<ProductBratch> ProductBratches { get; set; }
    }
}
