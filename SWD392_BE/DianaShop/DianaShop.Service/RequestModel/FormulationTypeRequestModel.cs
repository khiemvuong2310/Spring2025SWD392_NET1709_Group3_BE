using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class FormulationTypeRequestModel
    {
        [Required]
        public string Texture { get; set; } // Liquid, Cream, Gel, Powder, Stick, Spray, Foam, Oil, Balm
        [Required]
        public bool IsFragranceFree { get; set; }
        [Required]
        public bool IsAlcoholFree { get; set; }
        [Required]
        public bool IsParabenFree { get; set; }
        [Required]
        public bool IsEssentialOilFree { get; set; }
        [Required]
        public bool IsSiliconeFree { get; set; }
        [Required]
        public bool IsSulfateFree { get; set; }
    }
}
