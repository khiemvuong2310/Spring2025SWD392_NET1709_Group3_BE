using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class ProductSkinType
    {
        public int ProductId { get; set; }
        public int SkinTypeId { get; set; }
        public virtual Product Product { get; set; }
        public virtual SkinType SkinType { get; set; }
    }
}
