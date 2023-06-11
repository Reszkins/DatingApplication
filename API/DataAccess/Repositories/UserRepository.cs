using API.Models;

namespace API.DataAccess.Repositories
{
    public interface IUserRepository 
    {
        Task<int?> GetUserId(string email);
        Task<string?> GetUserEmail(int id);
        Task<UserBaseInfo?> GetUserBaseInfo(int userId);
        Task<string?> GetUserDescription(int userId);
        Task SetUserDescription(int userId, string description);
        Task DeleteUserDescription(int userId);
        Task SetUserRating(int userId, int targetUserId, int rating);
        Task<int?> GetUserRating(int userId, int targetUserId);
        Task<double?> GetUserRatingFromAll(int targetUserId);
    }

    public class UserRepository : IUserRepository
    {
        private readonly ISqlDataAccess _db;
        public UserRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<int?> GetUserId(string email)
        {
            var sql = "SELECT id FROM users_account WHERE email = @Email";
            var parameters = new Dictionary<string, object> { { "@Email", email } };

            var users = await _db.LoadData<int>(sql, parameters);

            return users.SingleOrDefault();
        }

        public async Task<string?> GetUserEmail(int id)
        {
            var sql = "SELECT email FROM users_account WHERE id = @Id";
            var parameters = new Dictionary<string, object> { { "@Id", id } };

            var users = await _db.LoadData<string>(sql, parameters);

            return users.SingleOrDefault();
        }

        public async Task<UserBaseInfo?> GetUserBaseInfo(int userId)
        {
            var sql = "SELECT * FROM users_base_info WHERE user_id = @UserId";
            var parameters = new Dictionary<string, object> { { "@UserId", userId } };

            var users = await _db.LoadData<UserBaseInfo>(sql, parameters);

            return users.FirstOrDefault();
        }

        public async Task<string?> GetUserDescription(int userId)
        {
            var sql = "SELECT description FROM users_base_info WHERE user_id = @UserId";
            var parameters = new Dictionary<string, object> { { "@UserId", userId } };

            var description = await _db.LoadData<string>(sql, parameters);

            return description.SingleOrDefault();
        }

        public async Task SetUserDescription(int userId, string description)
        {
            var sql = "UPDATE users_base_info SET description = @description WHERE user_id = @UserId";
            var parameters = new Dictionary<string, object> { { "@UserId", userId }, { "@description", description } };

            await _db.UpdateData(sql, parameters);
        }

        public async Task DeleteUserDescription(int userId)
        {
            var sql = "UPDATE users_base_info SET description = null WHERE user_id = @UserId";
            var parameters = new Dictionary<string, object> { { "@UserId", userId } };

            await _db.DeleteData(sql, parameters);
        }

        public async Task SetUserRating(int userId, int targetUserId, int rating)
        {
            var sql = "SELECT rating FROM users_behavior WHERE user_id = @UserId AND target_user_id = @TargetUserId";
            var parameters = new Dictionary<string, object> { { "@UserId", userId }, { "@TargetUserId", targetUserId } };

            var result = await _db.LoadData<int>(sql, parameters);

            if(result.Count == 0)
            {
                sql = "INSERT INTO users_behavior (user_id, target_user_id, rating) VALUES (@UserId, @TargetUserId, @Rating)";
                parameters = new Dictionary<string, object> { { "@UserId", userId }, { "@TargetUserId", targetUserId }, { "@Rating", rating } };

                await _db.SaveData(sql, parameters);
            }
            else
            {
                sql = "UPDATE users_behavior SET rating = @Rating WHERE user_id = @UserId AND target_user_id = @TargetUserId";
                parameters = new Dictionary<string, object> { { "@UserId", userId }, { "@TargetUserId", targetUserId }, { "@Rating", rating } };

                await _db.UpdateData(sql, parameters);
            }
        }

        public async Task<int?> GetUserRating(int userId, int targetUserId)
        {
            var sql = "SELECT rating FROM users_behavior WHERE user_id = @UserId AND target_user_id = @TargetUserId";
            var parameters = new Dictionary<string, object> { { "@UserId", userId }, { "@TargetUserId", targetUserId } };

            var result = await _db.LoadData<int>(sql, parameters);

            return result.FirstOrDefault();
        }

        public async Task<double?> GetUserRatingFromAll(int targetUserId)
        {
            var sql = "SELECT rating FROM users_behavior WHERE target_user_id = @TargetUserId";
            var parameters = new Dictionary<string, object> { { "@TargetUserId", targetUserId } };

            var result = await _db.LoadData<int>(sql, parameters);

            if (result is null) return null;

            var count = result.Count;
            var sum = 0;

            foreach(var rating in result)
            {
                sum += rating;
            }

            return sum/count;
        }
    }
}
