using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class OrderRequestModel
    {
        [Required]
        public int UserID { get; set; }
        public string Address { get; set; }

    }
}
