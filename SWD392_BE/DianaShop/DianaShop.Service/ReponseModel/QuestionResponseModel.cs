using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class QuestionResponseModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public List<AnswerResponseModel> Answers { get; set; }
    }
}
