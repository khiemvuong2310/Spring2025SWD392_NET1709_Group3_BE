using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class BrandRequestModel
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Origin { get; set; }
    }
}
