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
using AutoMapper.QueryableExtensions;

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
                .ProjectTo<QuestionResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return questions;
        }
        public async Task<List<AnswerResponseModel>> GetAllAnswerByQuestionIdAsync(int questionId)
        {
            var answers = await _unitOfWork.Repository<Answer>().GetAll()
                .Where(a => a.QuestionId == questionId && !a.IsDelete)
                .ProjectTo<AnswerResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return answers;
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

        public async Task<AnswerResponseModel> AddAnswerAsync(int questionId, AnswerRequestModel answerRequestModel)
        {
            var answer = _mapper.Map<Answer>(answerRequestModel);
            if (await _unitOfWork.Repository<SkinType>().AnyAsync(x => x.Id == answer.SkinTypeId) == false)
            {
                throw new ArgumentException("Skin type not found");
            }
            answer.QuestionId = questionId;
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
            if (questionRequestModel.Answers.Any())
            {
                foreach (var answer in question.Answers)
                {
                    if (await _unitOfWork.Repository<SkinType>().AnyAsync(x => x.Id == answer.SkinTypeId) == false)
                    {
                        throw new ArgumentException("Skin type not found");
                    }
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
            var newAnswer = _mapper.Map<Answer>(answerRequestModel);
            newAnswer.Id = answerId;
            newAnswer.QuestionId = answer.QuestionId;
            await _unitOfWork.Repository<Answer>().Update(newAnswer, answerId);
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
