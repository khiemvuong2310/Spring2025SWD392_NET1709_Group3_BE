using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class CommentRequestModel
    {
        public string Content { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
    public class CommentUpdateRequestModel
    {
        public string Content { get; set; }

    }
}
