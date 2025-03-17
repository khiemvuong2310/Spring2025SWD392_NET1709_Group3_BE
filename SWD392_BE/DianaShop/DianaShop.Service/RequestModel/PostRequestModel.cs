using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class PostRequestModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();
    }
}
