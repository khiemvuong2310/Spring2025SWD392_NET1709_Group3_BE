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
        public List<string> SkinTypes { get; set; }
        public List<RoutineStepResponseModel> RoutineSteps { get; set; }
    }
}
