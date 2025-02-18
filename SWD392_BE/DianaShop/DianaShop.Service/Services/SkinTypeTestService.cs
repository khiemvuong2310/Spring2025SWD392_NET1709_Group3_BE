using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace DianaShop.Service.Services
{
    public class SkinTypeTestService : ISkinTypeTestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SkinTypeTestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<AnswerResponseModel?> GetAnswerById(int id)
        {
            var answer = await _unitOfWork.Repository<Answer>().GetById(id);
            if (answer == null)
            {
                return null;
            }
            return _mapper.Map<AnswerResponseModel>(answer);
        }
        public async Task<QuestionResponseModel?> GetQuestionById(int id)
        {
            var question = await _unitOfWork.Repository<Question>().GetById(id);
            if (question == null)
            {
                return null;
            }
            return _mapper.Map<QuestionResponseModel>(question);
        }
        public async Task<List<QuestionResponseModel>> GetAllQuestionsAsync()
        {
            var questions = await _unitOfWork.Repository<Question>().GetAll()
                .Where(q => !q.IsDelete)
                .Include(q => q.Answers).ThenInclude(a => a.SkinType)
                .ToListAsync();
            return _mapper.Map<List<QuestionResponseModel>>(questions);
        }
        public async Task<List<AnswerResponseModel>> GetAllAnswerByQuestionIdAsync(int questionId)
        {
            var answers = await _unitOfWork.Repository<Answer>().GetAll()
                .Where(a => a.QuestionId == questionId && !a.IsDelete)
                .Include(a => a.SkinType)
                .ToListAsync();
            return _mapper.Map<List<AnswerResponseModel>>(answers);
        }
        public async Task<QuestionResponseModel> AddQuestionAsync(QuestionRequestModel questionRequestModel)
        {
            var question = _mapper.Map<Question>(questionRequestModel);
            foreach (var answer in question.Answers)
            {
                if (await _unitOfWork.Repository<SkinType>().AnyAsync(x => x.Id == answer.SkinTypeId) == false)
                {
                    throw new ArgumentException("Skin type not found");
                }
            }
            await _unitOfWork.Repository<Question>().InsertAsync(question);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<QuestionResponseModel>(question);
        }

        public async Task<AnswerResponseModel> AddAnswerAsync(AnswerRequestModel answerRequestModel)
        {
            var answer = _mapper.Map<Answer>(answerRequestModel);
            if (await _unitOfWork.Repository<SkinType>().AnyAsync(x => x.Id == answer.SkinTypeId) == false)
            {
                throw new ArgumentException("Skin type not found");
            }
            if (await GetQuestionById(answerRequestModel.QuestionId) == null)
            {
                throw new ArgumentException("Question not found");
            }
            await _unitOfWork.Repository<Answer>().InsertAsync(answer);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<AnswerResponseModel>(answer);
        }

        public async Task UpdateQuestionAsync(int questionId, QuestionRequestModel questionRequestModel)
        {
            var question = await _unitOfWork.Repository<Question>().GetById(questionId);
            if (question == null)
            {
                throw new ArgumentException("Question not found");
            }
            foreach (var answer in question.Answers)
            {
                if (await _unitOfWork.Repository<SkinType>().AnyAsync(x => x.Id == answer.SkinTypeId) == false)
                {
                    throw new ArgumentException("Skin type not found");
                }
            }
            _mapper.Map(questionRequestModel, question);
            await _unitOfWork.Repository<Question>().Update(question, questionId);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAnswerAsync(int answerId, AnswerRequestModel answerRequestModel)
        {
            var answer = await _unitOfWork.Repository<Answer>().GetById(answerId);
            if (answer == null)
            {
                throw new ArgumentException("Answer not found");
            }
            if (await _unitOfWork.Repository<SkinType>().AnyAsync(x => x.Id == answer.SkinTypeId) == false)
            {
                throw new ArgumentException("Skin type not found");
            }
            if (await GetQuestionById(answerRequestModel.QuestionId) == null)
            {
                throw new ArgumentException("Question not found");
            }
            _mapper.Map(answerRequestModel, answer);
            await _unitOfWork.Repository<Answer>().Update(answer, answerId);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteQuestionAsync(int questionId)
        {
            var question = await _unitOfWork.Repository<Question>().GetById(questionId);
            if (question == null)
            {
                throw new ArgumentException("Question not found");
            }
            question.IsDelete = true;
            await _unitOfWork.Repository<Question>().Update(question, questionId);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAnswerAsync(int answerId)
        {
            var answer = await _unitOfWork.Repository<Answer>().GetById(answerId);
            if (answer == null)
            {
                throw new ArgumentException("Answer not found");
            }
            answer.IsDelete = true;
            await _unitOfWork.Repository<Answer>().Update(answer, answerId);
            await _unitOfWork.CommitAsync();
        }
    }
}
