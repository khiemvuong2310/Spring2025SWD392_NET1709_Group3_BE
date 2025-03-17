using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class AnswerRequestModel
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int SkinTypeId { get; set; }
    }
}
