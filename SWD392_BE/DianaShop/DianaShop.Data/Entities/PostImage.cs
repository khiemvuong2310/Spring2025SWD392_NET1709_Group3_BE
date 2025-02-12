using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class PostImage
    {
        public int PostId { get; set; }
        public int ImageId { get; set; }
        public string? Caption { get; set; }
        public virtual Post Post { get; set; }
        public virtual Image Image { get; set; }
    }
}
