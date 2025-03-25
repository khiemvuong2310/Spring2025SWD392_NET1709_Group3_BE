using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class SkincareRoutineResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public List<RoutineSkinTypeResponse> SkinTypes { get; set; }
        public List<RoutineStepResponseModel> RoutineSteps { get; set; }
    }
    public class RoutineSkinTypeResponse
    {
        public int SkinTypeId { get; set; }
        public string SkinTypeName { get; set; }
        [Precision(3, 0)]
        public decimal Percentage { get; set; }
    }
}
