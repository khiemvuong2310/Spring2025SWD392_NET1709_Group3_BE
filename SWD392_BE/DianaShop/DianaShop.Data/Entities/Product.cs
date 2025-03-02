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
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int PackagingId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
        public int FormulationTypeId { get; set; }
        public string Direction { get; set; }
        public string PAO { get; set; } //period after opening (12M , 24M, 36M)
        public string Precaution { get; set; }
        public string Storage { get; set; }
        public string AdditionalInformation { get; set; }
        public bool Status { get; set; } // true: available, false: out of stock
        public virtual Category Category { get; set; }
        public virtual FormulationType FormulationType { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Packaging Packaging { get; set; }

        public virtual ICollection<ProductBatch> ProductBatches { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductIngredient> ProductIngredients { get; set; }
        public virtual ICollection<ProductSkinType> ProductSkinTypes { get; set; }

        public virtual ICollection<CartProduct> CartProducts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
