using DianaShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class SkincareRoutineRequestModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public List<RoutineSkinTypeRequest> RoutineSkinTypes { get; set; }
        public List<RoutineStepRequestModel> RoutineSteps { get; set; } = new List<RoutineStepRequestModel>();

    }
    public class RoutineSkinTypeRequest
    {
        [Required]
        public int SkinTypeId { get; set; }
        [Required]
        [Range(0, 100)]
        public decimal Percentage { get; set; }
    }
}
