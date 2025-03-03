using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class LoginResponseModel
    {
        public string Token { get; set; }

        public UserResponseModel User { get; set; }
    }
}
