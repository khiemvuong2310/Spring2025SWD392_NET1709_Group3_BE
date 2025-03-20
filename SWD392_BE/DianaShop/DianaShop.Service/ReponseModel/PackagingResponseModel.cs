using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class PackagingResponseModel
    {
        public int Id { get; set; } // From BaseEntity

        public string Type { get; set; }

        public string Material { get; set; }

        public string Size { get; set; }

        public bool IsRefillable { get; set; }
    }
}
