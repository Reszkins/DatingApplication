using API.DataAccess.Repositories;
using API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/questions")]
    [Authorize]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;

        public QuestionsController(IQuestionRepository questionRepository, IUserRepository userRepository)
        {
            _questionRepository = questionRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions()
        {
            var questions = await _questionRepository.GetQuestions();

            var questionsDto = new List<QuestionDto>();

            foreach(var question in questions)
            {
                questionsDto.Add(new QuestionDto
                {
                    QuestionNumber = question.QuestionNumber,
                    QuestionText = question.QuestionText
                });
            }

            return Ok(questionsDto);
        }

        [HttpPost("answer")]
        public async Task<IActionResult> SaveAnswer(int questionId, int answer)
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));

            if (userId is null) return Ok(500);

            if (answer < 1 || answer > 5) return BadRequest("Answer has wrong value");

            await _questionRepository.SaveAnswer(questionId, answer, userId.Value);

            return Ok();
        }
    }
}
