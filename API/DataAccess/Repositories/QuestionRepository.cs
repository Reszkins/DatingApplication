using API.Dtos;
using API.Models;

namespace API.DataAccess.Repositories
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetQuestions();
        Task SaveAnswer(int questionId, int answer, int userId);
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

        public async Task SaveAnswer(int questionId, int answer, int userId)
        {
            var sql = "SELECT user_id FROM questionnaires WHERE user_id = @UserId";

            var parameters = new Dictionary<string, object> {
                { "@UserId", userId }
            };

            var result = await _db.LoadData<int>(sql, parameters);
            if(result.Count == 0)
            {
                sql = $"INSERT INTO questionnaires (user_id, q{questionId}) VALUES (@UserId, @Answer)";

                parameters = new Dictionary<string, object> {
                    { "@UserId", userId },
                    { "@Answer", answer }
                };

                await _db.SaveData(sql, parameters);
            }
            else
            {
                sql = $"UPDATE questionnaires SET q{questionId} = @Answer WHERE user_id = @UserId";

                parameters = new Dictionary<string, object> {
                    { "@UserId", userId },
                    { "@Answer", answer }
                };

                await _db.UpdateData(sql, parameters);
            }
        }
    }
}
