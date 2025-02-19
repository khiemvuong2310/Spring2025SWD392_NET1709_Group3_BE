using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public string? Image { get; set; }
        public bool? Status { get; set; }
        public int SkinTypeId { get; set; }
        public string SkinTypeName { get; set; }

        public CartResponseModel? Cart { get; set; }
        public List<VoucherStorageResponseModel>? VoucherStorage { get; set; }
        public List<UserRoleResponseModel>? UserRoles { get; set; }
        public List<FeedBackResponseModel>? FeedBacks { get; set; }
        public List<OrderResponseModel>? Orders { get; set; }

    }
}
