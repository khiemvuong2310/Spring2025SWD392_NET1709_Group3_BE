using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public int CategoryID { get; set; }

        public double Price { get; set; }

        public string Brand { get; set; }

        public int QuantityInStock { get; set; }

        public string imageURL { get; set; }

        public string? Status { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
