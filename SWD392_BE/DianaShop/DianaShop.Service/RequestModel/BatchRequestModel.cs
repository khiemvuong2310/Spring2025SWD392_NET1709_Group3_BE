using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class BatchRequestModel : IValidatableObject
    {
        [Required]
        public int ProductID { get; set; }
        [Required]

        public int Quantity { get; set; }
        [Required]

        public DateTime ExpiryDate { get; set; }
        [Required]

        public DateTime ImportDate { get; set; }
        [Required]
        public DateTime ManufactureDate { get; set; }
        [Required]
        public bool Status { get; set; } // true: active, false: expired

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();
            if (ExpiryDate < DateTime.Today)
            {
                ValidationResult mss = new ValidationResult("Expire date must be greater than or equal to today");
                res.Add(mss);

            }
            if (ImportDate > DateTime.Today)
            {
                ValidationResult mss = new ValidationResult("Import date must be less than or equal to today");
                res.Add(mss);
            }
            if (ManufactureDate > DateTime.Today)
            {
                ValidationResult mss = new ValidationResult("Manufacture date must be less than or equal to today");
                res.Add(mss);
            }
            if (ExpiryDate < ManufactureDate)
            {
                ValidationResult mss = new ValidationResult("Expire date must be greater than manufacture date");
                res.Add(mss);
            }
            if (ImportDate < ManufactureDate)
            {
                ValidationResult mss = new ValidationResult("Import date must be greater than manufacture date");
                res.Add(mss);
            }
            return res;
        }
    }
}
