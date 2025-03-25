using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class RoutineSkinType
    {
        public int SkinTypeId { get; set; }
        public int RoutineId { get; set; }
        [Precision(3, 2)]
        public decimal Percentage { get; set; }
        public virtual SkinType SkinType { get; set; }
        public virtual SkincareRoutine Routine { get; set; }
        
    }
}
