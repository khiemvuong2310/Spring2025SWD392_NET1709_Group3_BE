using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Answer : BaseEntity
    {
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public int SkinTypeId { get; set; }
        public virtual Question Question { get; set; }
        public virtual SkinType SkinType { get; set; }
    }
}
