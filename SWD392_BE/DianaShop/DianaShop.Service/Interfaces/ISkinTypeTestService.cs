using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface ISkinTypeTestService
    {
        Task<AnswerResponseModel?> GetAnswerById(int id);
        Task<QuestionResponseModel?> GetQuestionById(int id);
        Task<List<AnswerResponseModel>> GetAllAnswerByQuestionIdAsync(int questionId);
        Task<QuestionResponseModel> AddQuestionAsync(QuestionRequestModel questionRequestModel);
        Task<AnswerResponseModel> AddAnswerAsync(int questionId, AnswerRequestModel answerRequestModel);
        Task<List<QuestionResponseModel>> GetAllQuestionsAsync();
        Task UpdateQuestionAsync(int questionId, QuestionRequestModel questionRequestModel);
        Task UpdateAnswerAsync(int answerId, AnswerRequestModel answerRequestModel);
        Task DeleteQuestionAsync(int questionId);
        Task DeleteAnswerAsync(int answerId);
    }
}
