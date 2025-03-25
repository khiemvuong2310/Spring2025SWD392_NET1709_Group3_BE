using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class UserSkinType
    {
        public int SkinTypeId { get; set; }
        public int UserId { get; set; }
        [Precision(3, 2)]
        public decimal Percentage { get; set; }
        public virtual SkinType SkinType { get; set; }
        public virtual User User { get; set; }
    }
}
