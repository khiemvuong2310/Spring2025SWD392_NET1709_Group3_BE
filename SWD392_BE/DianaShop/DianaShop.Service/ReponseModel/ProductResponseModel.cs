using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class ProductResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Brand { get; set; } // Tên của thương hiệu
        public string Category { get; set; } // Tên của danh mục
        public string Packaging { get; set; } // Tên của bao bì
        public decimal? Price { get; set; }
        public string FormulationType { get; set; } // Loại công thức (ví dụ: Lotion, Cream, Gel...)
        public string Direction { get; set; }
        public string PAO { get; set; } // Period After Opening (12M, 24M, 36M)
        public string Precaution { get; set; }
        public string Storage { get; set; }
        public string AdditionalInformation { get; set; }
        public int StockQuantity { get; set; }
        public bool Status { get; set; } // true: available, false: out of stock
        public bool IsDeleted { get; set; }
        public List<int> ProductBatchIds { get; set; } // List of Batch Codes
        public List<string> ProductTags { get; set; } // List of tags like "organic", "vegan", etc.
        public List<string> ProductImages { get; set; } // List of image URLs
        public List<string> ProductIngredients { get; set; } // List of ingredients
        public List<string> ProductSkinTypes { get; set; } // List of compatible skin types
    }

    public class ProductResponseCart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }
}
