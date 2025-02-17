using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public string? Token { get; set; }
        public string? Image { get; set; }
        public bool? Status { get; set; }
        public int SkinTypeId { get; set; }
        public virtual SkinType SkinType { get; set; }

        public virtual ICollection<UserRole>? UserRoles { get; set; }

    }
}
