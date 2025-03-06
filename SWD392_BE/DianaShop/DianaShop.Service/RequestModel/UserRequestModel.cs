using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class UserCreateRequest
    {
        [RegularExpression(@"^[\p{L}0-9\s]*$", ErrorMessage = "Username cannot contain special characters.")]
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Phone number must be between 10 and 15 digits and may start with a '+' sign.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public string Location { get; set; }

        //[Required(ErrorMessage = "Skin type is required.")]
        public int? SkinTypeId { get; set; }

        public bool Status { get; set; } = true; // Mặc định là true
    }
    public class UserUpdateRequest
    {
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Username cannot contain special characters.")] public string? Username { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(6)]
        public string? Password { get; set; }

        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Phone number must be between 10 and 15 digits.")]
        public string? Phone { get; set; }

        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public string? Location { get; set; }

        public string? Image { get; set; }

        public bool? Status { get; set; }

        public int? SkinTypeId { get; set; }
        public List<int>? RoleIds { get; set; }

    }

    public class UpdatePasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class UserUpdateWallet
    {
        public decimal? Wallet { get; set; }
    }

}
