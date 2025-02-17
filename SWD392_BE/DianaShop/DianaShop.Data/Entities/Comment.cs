using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int LikeCount { get; set; }
        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
    }
}
