using ElearningQuiz.Core.DTO;
using ElearningQuiz.Core.Configuration;
using ElearningQuiz.Core.DTO.User;
using ElearningQuiz.Core.Entities;
using ElearningQuiz.Core.Enums;
using ElearningQuiz.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ElearningQuiz.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthService(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IConfiguration configuration,
            IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
        {
            // 1. Fetch user by email
            var users = await _unitOfWork.Repository<User>().FindAsync(u => u.Email == request.Email);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                return null; // User not found
            }

            // 2. Verify Password
            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return null; // Invalid credentials
            }

            return CreateAuthResponse(user.UserId, user.Email, user.FirstName, user.LastName, user.Role);
        }

        public async Task<AuthResponseDto?> RegisterAsync(StudentRegistrationRequestDto request)
        {
            var email = request.Email.Trim();
            var existingUsers = await _unitOfWork.Repository<User>()
                .FindAsync(user => user.Email.ToLower() == email.ToLower());

            if (existingUsers.Any())
            {
                return null;
            }

            var user = await _userService.CreateUserAsync(new CreateUserDto
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = email,
                PasswordHash = request.Password,
                MobileNo = request.MobileNo.Trim(),
                DepartmentId = request.DepartmentId,
                BatchId = request.BatchId,
                Role = UserRole.Student
            });

            return CreateAuthResponse(user.UserId, user.Email, user.FirstName, user.LastName, user.Role);
        }

        private AuthResponseDto CreateAuthResponse(string userId, string email, string firstName, string lastName, UserRole role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecret = _configuration["JwtSettings:SecretKey"] ?? JwtDefaults.FallbackSecretKey;
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var expiresAt = DateTime.UtcNow.AddHours(8);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, $"{firstName} {lastName}"),
                    new Claim(ClaimTypes.Role, role.ToString())
                }),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                UserId = userId,
                Email = email,
                FullName = $"{firstName} {lastName}",
                Role = role.ToString(),
                ExpiresAt = expiresAt
            };
        }
    }
}