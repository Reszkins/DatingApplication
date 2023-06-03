using API.Dtos;
using API.Models;

namespace API.DataAccess.Repositories
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetQuestions();
        Task SaveAnswer(List<AnswerDto> answers, int userId);
    }

    public class QuestionRepository : IQuestionRepository
    {
        private readonly ISqlDataAccess _db;
        public QuestionRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<Question>> GetQuestions()
        {
            var sql = "SELECT * FROM questions";

            var questions = await _db.LoadData<Question>(sql, new Dictionary<string, object>());

            return questions.ToList();
        }

        public async Task SaveAnswer(List<AnswerDto> answers, int userId)
        {
            var sql = "SELECT user_id FROM users_matching_info WHERE user_id = @UserId";

            var parameters = new Dictionary<string, object> {
                { "@UserId", userId }
            };

            var result = await _db.LoadData<int>(sql, parameters);

            if(result.Count == 0)
            {
                sql = $"INSERT INTO users_matching_info (user_id) VALUES (@UserId)";

                parameters = new Dictionary<string, object> {
                    { "@UserId", userId },
                };

                await _db.SaveData(sql, parameters);
            }

            foreach(var answer in answers)
            {
                sql = "SELECT matching_info_column_name FROM questions WHERE question_number = @QuestionNumber";

                parameters = new Dictionary<string, object> {
                    { "@QuestionNumber", answer.QuestionNumber }
                };

                var response = await _db.LoadData<string>(sql, parameters);
                var column = response.FirstOrDefault();


                sql = $"UPDATE users_matching_info SET {column} = @Answer WHERE user_id = @UserId";

                parameters = new Dictionary<string, object> {
                    { "@UserId", userId },
                    { "@Answer", answer.Answer }
                };

                await _db.UpdateData(sql, parameters);
            };
        }
    }
}
