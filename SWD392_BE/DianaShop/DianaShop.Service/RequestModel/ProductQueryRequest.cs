using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class ProductQueryRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? FormulationTypeId { get; set; }
        public int? PackagingId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public ProductSort SortBy { get; set; } = ProductSort.Default;
        public bool Ascending { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public List<int> SkinTypeIds { get; set; } = new List<int>();
        public List<int> TagIds { get; set; } = new List<int>();
        public List<int> IngredientIds { get; set; } = new List<int>();
    }
    public enum ProductSort
    {
        [Display(Name = "Default")]
        Default, //id
        [Display(Name = "Name")]
        Name,
        [Display(Name = "Price")]
        Price,
        [Display(Name = "Stock Quantity")]
        StockQuantity
    }
}

