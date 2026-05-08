using FluentValidation;
using FluentValidation.Results;
using Library_Management_System.DTOs.Auth;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interfaces;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_Management_System.Services
{
    public class AuthService(
        IRepository<User> users,
        IConfiguration configuration) : IAuthService
    {
        private readonly PasswordHasher<User> _passwordHasher = new();

        #region Commands

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await users.Query()
                .FirstOrDefaultAsync(user => user.Email == dto.Email);

            if (user is null || user.IsDeleted || !user.IsActive)
            {
                return null;
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return CreateAuthResponse(user);
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var emailExists = await users.ExistsAsync(user => user.Email == dto.Email && !user.IsDeleted);

            if (emailExists)
            {
                throw new ValidationException(
                [
                    new FluentValidation.Results.ValidationFailure(nameof(dto.Email), "A user with this email already exists.")
                ]);
            }

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Role = dto.Role,
                IsActive = true
            };

            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            await users.AddAsync(user);
            await users.SaveChangesAsync();

            return CreateAuthResponse(user);
        }

        #endregion

        #region Token

        private AuthResponseDto CreateAuthResponse(User user)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(GetTokenExpiryMinutes());

            return new AuthResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = CreateJwtToken(user, expiresAt),
                ExpiresAt = expiresAt
            };
        }

        private string CreateJwtToken(User user, DateTime expiresAt)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetJwtKey()));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GetJwtKey()
        {
            return configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key is missing from configuration.");
        }

        private int GetTokenExpiryMinutes()
        {
            return configuration.GetValue("Jwt:ExpiryMinutes", 60);
        }

        #endregion
    }
}
