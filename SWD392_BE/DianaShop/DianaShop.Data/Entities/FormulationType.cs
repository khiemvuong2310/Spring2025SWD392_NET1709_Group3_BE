using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class FormulationType : BaseEntity
    {
        public string Texture { get; set; } // Liquid, Cream, Gel, Powder, Stick, Spray, Foam, Oil, Balm
        public bool IsFragranceFree { get; set; }
        public bool IsAlcoholFree { get; set; }
        public bool IsParabenFree { get; set; }
        public bool IsEssentialOilFree { get; set; }
        public bool IsSiliconeFree { get; set; }
        public bool IsSulfateFree { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
