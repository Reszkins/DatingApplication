﻿using API.DataAccess.Repositories;
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
        public async Task<IActionResult> SaveAnswer([FromBody] AnswersDto answersDto)
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));

            if (userId is null) return Ok(500);

            var answers = answersDto.Answers;

            foreach(var answer in answers)
            {
                if (answer.Answer < 1 || answer.Answer > 5) return BadRequest("Answer has wrong value");
            }

            await _questionRepository.SaveAnswer(answers, userId.Value);

            return Ok();
        }
    }
}
