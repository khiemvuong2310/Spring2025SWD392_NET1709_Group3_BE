using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    public class SkinTypeTestController : BaseController
    {
        private readonly ISkinTypeTestService _skinTypeTestService;
        public SkinTypeTestController(ISkinTypeTestService skinTypeTestService)
        {
            _skinTypeTestService = skinTypeTestService;
        }

        // POST api/skintypetest
        [HttpPost("questions")]
        public async Task<IActionResult> AddQuestionAsync([FromBody] QuestionRequestModel questionRequestModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _skinTypeTestService.AddQuestionAsync(questionRequestModel);
            return Ok(response);
        }

        [HttpPost("answers")]
        public async Task<IActionResult> AddAnswerAsync([FromBody] AnswerRequestModel answerRequestModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _skinTypeTestService.AddAnswerAsync(answerRequestModel);
            return Ok(response);
        }

        [HttpGet("questions")]
        public async Task<IActionResult> GetAllQuestionsAsync()
        {
            var response = await _skinTypeTestService.GetAllQuestionsAsync();
            return Ok(response);
        }

        [HttpGet("questions/{questionId}")]
        public async Task<IActionResult> GetQuestionById(int questionId)
        {
            var response = await _skinTypeTestService.GetQuestionById(questionId);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("questions/{questionId}/answers")]
        public async Task<IActionResult> GetAllAnswerByQuestionIdAsync(int questionId)
        {
            var response = await _skinTypeTestService.GetAllAnswerByQuestionIdAsync(questionId);
            return Ok(response);
        }

        [HttpPut("questions/{questionId}")]
        public async Task<IActionResult> UpdateQuestionAsync(int questionId, [FromBody] QuestionRequestModel questionRequestModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _skinTypeTestService.UpdateQuestionAsync(questionId, questionRequestModel);
            return NoContent();
        }

        [HttpPut("answers/{answerId}")]
        public async Task<IActionResult> UpdateAnswerAsync(int answerId, [FromBody] AnswerRequestModel answerRequestModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _skinTypeTestService.UpdateAnswerAsync(answerId, answerRequestModel);
            return NoContent();
        }

        [HttpDelete("questions/{questionId}")]
        public async Task<IActionResult> DeleteQuestionAsync(int questionId)
        {
            await _skinTypeTestService.DeleteQuestionAsync(questionId);
            return Ok();
        }

        [HttpDelete("answers/{answerId}")]
        public async Task<IActionResult> DeleteAnswerAsync(int answerId)
        {
            await _skinTypeTestService.DeleteAnswerAsync(answerId);
            return Ok();
        }
    }
}
