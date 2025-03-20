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
    public class QuestionMapping : Profile
    {
        public QuestionMapping()
        {
            CreateMap<Question, QuestionResponseModel>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers.Where(a => !a.IsDelete)));
            CreateMap<QuestionRequestModel, Question>();          
        }
    }
}
