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
            
            // Convert to Vietnam timezone (UTC+7)
            var vietnamTime = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var currentVietnamTime = TimeZoneInfo.ConvertTime(DateTime.Now, vietnamTime);
            var todayVietnam = new DateTime(currentVietnamTime.Year, currentVietnamTime.Month, currentVietnamTime.Day);

            // Convert input dates to Vietnam timezone  
            var expiryDateVietnam = TimeZoneInfo.ConvertTime(ExpiryDate, vietnamTime);
            var importDateVietnam = TimeZoneInfo.ConvertTime(ImportDate, vietnamTime);
            var manufactureDateVietnam = TimeZoneInfo.ConvertTime(ManufactureDate, vietnamTime);

            if (expiryDateVietnam.Date < todayVietnam.Date)
            {
                res.Add(new ValidationResult("Expire date must be greater than or equal to today"));
            }
            if (importDateVietnam.Date > todayVietnam.Date)
            {
                res.Add(new ValidationResult("Import date must be less than or equal to today"));
            }
            if (manufactureDateVietnam.Date > todayVietnam.Date)
            {
                res.Add(new ValidationResult("Manufacture date must be less than or equal to today"));
            }
            if (expiryDateVietnam.Date < manufactureDateVietnam.Date)
            {
                res.Add(new ValidationResult("Expire date must be greater than manufacture date"));
            }
            if (importDateVietnam.Date < manufactureDateVietnam.Date)
            {
                res.Add(new ValidationResult("Import date must be greater than manufacture date"));
            }
            return res;
        }
    }
}
