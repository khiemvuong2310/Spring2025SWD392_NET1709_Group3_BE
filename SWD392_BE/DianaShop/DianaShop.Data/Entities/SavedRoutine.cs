using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class SavedRoutine
    {
        public int UserId { get; set; }
        public int RoutineId { get; set; }
        public virtual SkincareRoutine Routine { get; set; }
        public virtual User User { get; set; }
    }
}
