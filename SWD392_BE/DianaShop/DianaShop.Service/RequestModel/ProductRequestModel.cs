using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class ProductRequestModel
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int PackagingId { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public int FormulationTypeId { get; set; }
        [Required]
        public string Direction { get; set; }
        [Required]
        public string PAO { get; set; } // Period After Opening (12M, 24M, 36M)
        [Required]
        public string Precaution { get; set; }
        [Required]
        public string Storage { get; set; }
        public string AdditionalInformation { get; set; }
        public bool Status { get; set; } // true: available, false: out of stock
        public List<int> ProductBatchIds { get; set; } = new List<int>(); // List of ProductBatchIds
        public List<int> ProductTagIds { get; set; } = new List<int>(); // List of ProductTagIds
        public List<IFormFile> ProductImageFiles { get; set; } = new List<IFormFile>(); // List of ProductImageIds
        public List<int> ProductIngredientIds { get; set; } = new List<int>(); // List of ProductIngredientIds
        public List<int> ProductSkinTypeIds { get; set; } = new List<int>(); // List of ProductSkinTypeIds
    }
}
