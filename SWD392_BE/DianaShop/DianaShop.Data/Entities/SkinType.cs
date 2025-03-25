using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class SkinType : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<ProductSkinType> ProductSkinTypes { get; set; }
        public virtual ICollection<RoutineSkinType> RoutineSkinTypes { get; set; }
        public virtual ICollection<UserSkinType> UserSkinTypes { get; set; }
    }
}
