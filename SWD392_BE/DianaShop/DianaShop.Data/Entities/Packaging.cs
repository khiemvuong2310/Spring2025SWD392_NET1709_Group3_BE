using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Packaging : BaseEntity
    {
        public string Type { get; set; } //jar, tube, pump bottle, dropper
        public string Material { get; set; }
        public string Size { get; set; }
        public bool IsRefillable { get; set; }
    }
}
