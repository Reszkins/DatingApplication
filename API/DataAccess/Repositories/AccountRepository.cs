﻿using API.Models;

namespace API.DataAccess.Repositories
{
    public interface IAccountRepository
    {
        Task<UserAccount?> GetUser(string email);
        Task<UserAccount?> GetUser(int id);
        Task AddNewAccount(UserAccount newAccount, UserBaseInfo newAccountBaseInfo);
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly ISqlDataAccess _db;
        public AccountRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<UserAccount?> GetUser(string email)
        {
            var sql = "SELECT id, email, password_hash, password_salt FROM users_account WHERE email = @Email";
            var parameters = new Dictionary<string, object> { { "@Email", email } };

            var users = await _db.LoadData<UserAccount>(sql, parameters);

            return users.FirstOrDefault();
        }

        public async Task<UserAccount?> GetUser(int id)
        {
            var sql = "SELECT id, email, password_hash, password_salt FROM users_account WHERE id = @Id";
            var parameters = new Dictionary<string, object> { { "@Id", id } };

            var users = await _db.LoadData<UserAccount>(sql, parameters);

            return users.FirstOrDefault();
        }

        public async Task AddNewAccount(UserAccount newAccount, UserBaseInfo newAccountBaseInfo)
        {
            var sql = "INSERT INTO users_account (email, password_hash, password_salt) VALUES (@Email, @PasswordHash, @PasswordSalt)";
            var parameters = new Dictionary<string, object> {
                { "@Email", newAccount.Email },
                { "@PasswordHash", newAccount.PasswordHash },
                { "@PasswordSalt", newAccount.PasswordSalt }
            };
            await _db.SaveData(sql, parameters);

            var userInfo = GetUser(newAccount.Email);

            sql = "INSERT INTO users_base_info (user_id, first_name, second_name, gender, sexuality, date_of_birth, description) VALUES (@UserId, @FirstName, @SecondName, @Gender, @Sexuality, @DateOfBirth, null)";
            parameters = new Dictionary<string, object> {
                { "@UserId", userInfo.Id },
                { "@FirstName", newAccountBaseInfo.FirstName },
                { "@SecondName", newAccountBaseInfo.SecondName },
                { "@Sexuality", newAccountBaseInfo.Sexuality },
                { "@Gender", newAccountBaseInfo.Gender },
                { "@DateOfBirth", newAccountBaseInfo.DateOfBirth }
            };
            await _db.SaveData(sql, parameters);
        }
    }
}
