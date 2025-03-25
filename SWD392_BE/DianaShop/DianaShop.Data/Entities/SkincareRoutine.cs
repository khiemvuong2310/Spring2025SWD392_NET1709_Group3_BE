using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class SkincareRoutine : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<RoutineSkinType> RoutineSkinTypes { get; set; }
        public virtual List<RoutineStep> RoutineSteps { get; set; }
        public virtual ICollection<SavedRoutine> SavedRoutines { get; set; }
    }
}
