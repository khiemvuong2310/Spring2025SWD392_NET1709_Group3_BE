using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class FeedBack :BaseEntity
    {
        public int UserID { get; set; }

        public int ProductID { get; set; } 
        public int? Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ReplyOn {  get; set; }
        
        public virtual Product Product { get; set; }

        public virtual User User { get; set; }  
    }
}
