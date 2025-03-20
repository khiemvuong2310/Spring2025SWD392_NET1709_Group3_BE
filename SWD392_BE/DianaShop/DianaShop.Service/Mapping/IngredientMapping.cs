using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Mapping
{
    public class IngredientMapping : Profile
    {
        public IngredientMapping()
        {
            CreateMap<IngredientRequestModel, Ingredient>();
            CreateMap<Ingredient, IngredientResponseModel>();
        }
    }
}
