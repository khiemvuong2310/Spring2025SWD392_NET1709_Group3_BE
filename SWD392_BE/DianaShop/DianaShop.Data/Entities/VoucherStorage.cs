using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class VoucherStorage :BaseEntity
    {
        public int UserID { get; set; }

        public virtual ICollection<Storage>? Storages { get; set; }
        public virtual User User { get; set; }
    }
}
