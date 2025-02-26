using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class ImageResponseModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public List<int> PostIds { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
