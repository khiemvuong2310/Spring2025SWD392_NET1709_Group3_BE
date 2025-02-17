using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class ProductRequestModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int PackagingId { get; set; }
        public decimal? Price { get; set; }
        public int FormulationTypeId { get; set; }
        public string Direction { get; set; }
        public string PAO { get; set; } // Period After Opening (12M, 24M, 36M)
        public string Precaution { get; set; }
        public string Storage { get; set; }
        public string AdditionalInformation { get; set; }
        public bool Status { get; set; } // true: available, false: out of stock
        public List<int> ProductBatches { get; set; } = new List<int>(); // List of ProductBatchIds
        public List<int> ProductTags { get; set; } = new List<int>(); // List of ProductTagIds
        public List<int> ProductImages { get; set; } = new List<int>(); // List of ProductImageIds
        public List<int> ProductIngredients { get; set; } = new List<int>(); // List of ProductIngredientIds
        public List<int> ProductSkinTypes { get; set; } = new List<int>(); // List of ProductSkinTypeIds
    }
}
