using API.DataAccess.Repositories;
using API.Dtos;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenGeneratorService _tokenGenerator;
        public AuthController(IAccountRepository accountRepository, ITokenGeneratorService tokenGenerator)
        {
            _accountRepository = accountRepository;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto credentials)
        {
            var user = await _accountRepository.GetUser(credentials.Email);

            if (user is null) return BadRequest("Invalid username or password.");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(credentials.Password));

            for (int i = 0; i < computedHash.Length; ++i)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid username or password.");
            }

            return Ok(new { Token = _tokenGenerator.GenerateToken(credentials.Email) });
        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto credentials)
        {
            if (await _accountRepository.GetUser(credentials.Email) is not null) return BadRequest("Invalid username or password.");

            using var hmac = new HMACSHA512();

            var newAccount = new UserAccount
            {
                Email = credentials.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(credentials.Password)),
                PasswordSalt = hmac.Key
            };

            var newAccountBaseInfo = new UserBaseInfo 
            {
                FirstName = credentials.FirstName,
                SecondName = credentials.SecondName,
                Gender = credentials.Gender,
                DateOfBirth = credentials.DateOfBirth
            };

            await _accountRepository.AddNewAccount(newAccount, newAccountBaseInfo);

            return Ok(new { Token = _tokenGenerator.GenerateToken(newAccount.Email) });
        }
    }
}
