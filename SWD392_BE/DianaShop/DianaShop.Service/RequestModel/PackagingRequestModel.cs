using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class PackagingRequestModel
    {
        [Required]
        public string Type { get; set; } // jar, tube, pump bottle, dropper

        [Required]
        public string Material { get; set; }

        [Required]
        public string Size { get; set; }

        public bool IsRefillable { get; set; }
    }
}
