using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class RoutineStep : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int SkincareRoutineId { get; set; }
        public virtual SkincareRoutine SkincareRoutine { get; set; }
    }
}
