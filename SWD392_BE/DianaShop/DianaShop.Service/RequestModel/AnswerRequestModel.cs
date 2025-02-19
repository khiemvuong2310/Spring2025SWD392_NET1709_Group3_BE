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
        public int QuestionId { get; set; } // The ID of the related question
        [Required]
        public int SkinTypeId { get; set; } // The ID of the related skin type
    }
}
