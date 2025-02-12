using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class RoutineSkinType
    {
        public int SkinTypeId { get; set; }
        public int RoutineId { get; set; }
        public virtual SkinType SkinType { get; set; }
        public virtual SkincareRoutine Routine { get; set; }
    }
}
