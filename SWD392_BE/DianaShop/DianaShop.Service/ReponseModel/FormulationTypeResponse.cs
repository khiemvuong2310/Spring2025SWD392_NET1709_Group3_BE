using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class FormulationTypeResponse
    {
        public int Id { get; set; }
        public string Texture { get; set; }
        public bool IsFragranceFree { get; set; }
        public bool IsAlcoholFree { get; set; }
        public bool IsParabenFree { get; set; }
        public bool IsEssentialOilFree { get; set; }
        public bool IsSiliconeFree { get; set; }
        public bool IsSulfateFree { get; set; }
        public List<string> ProductNames { get; set; }
    }
}
