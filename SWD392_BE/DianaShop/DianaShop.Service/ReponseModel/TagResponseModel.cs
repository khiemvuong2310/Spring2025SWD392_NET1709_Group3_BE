using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class TagResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Products { get; set; }
        public List<string> Posts { get; set; }
    }
}
