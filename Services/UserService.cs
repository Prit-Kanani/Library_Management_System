using Library_Management_System.DTOs.Common;
using Library_Management_System.DTOs.Users;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interfaces;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services
{
    public class UserService(IRepository<User> users) : IUserService
    {
        private readonly PasswordHasher<User> _passwordHasher = new();

        #region Query

        public async Task<PagedResultDto<UserViewDto>> GetAllAsync(UserFilterDto filter)
        {
            var query = users.Query().AsNoTracking();

            if (!filter.IncludeDeleted)
            {
                query = query.Where(user => !user.IsDeleted);
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(user =>
                    user.FullName.Contains(filter.Search) ||
                    user.Email.Contains(filter.Search));
            }

            if (!string.IsNullOrWhiteSpace(filter.Role))
            {
                query = query.Where(user => user.Role == filter.Role);
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(user => user.IsActive == filter.IsActive.Value);
            }

            return await query
                .OrderBy(user => user.FullName)
                .Select(user => ToViewDto(user))
                .ToPagedResultAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<UserViewDto?> GetByIdAsync(int id)
        {
            return await users.Query()
                .AsNoTracking()
                .Where(user => user.UserId == id)
                .Select(user => ToViewDto(user))
                .FirstOrDefaultAsync();
        }

        public async Task<UserUpdateDto?> GetByIdForUpdateAsync(int id)
        {
            return await users.Query()
                .AsNoTracking()
                .Where(user => user.UserId == id)
                .Select(user => new UserUpdateDto
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Password = string.Empty,
                    Role = user.Role,
                    IsActive = user.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<DropdownDto>> GetDropdownAsync()
        {
            return await users.Query()
                .AsNoTracking()
                .Where(user => user.IsActive && !user.IsDeleted)
                .OrderBy(user => user.FullName)
                .Select(user => new DropdownDto
                {
                    Id = user.UserId,
                    Text = user.FullName
                })
                .ToListAsync();
        }

        #endregion

        #region Commands

        public async Task<UserViewDto> CreateAsync(UserCreateDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Role = dto.Role,
                IsActive = dto.IsActive
            };

            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            await users.AddAsync(user);
            await users.SaveChangesAsync();

            return ToViewDto(user);
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateDto dto)
        {
            var user = await users.GetByIdAsync(id);

            if (user is null || user.IsDeleted)
            {
                return false;
            }

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Password = _passwordHasher.HashPassword(user, dto.Password);
            user.Role = dto.Role;
            user.IsActive = dto.IsActive;

            users.Update(user);
            await users.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await users.GetByIdAsync(id);

            if (user is null || user.IsDeleted)
            {
                return false;
            }

            user.IsDeleted = true;
            user.IsActive = false;
            users.Update(user);
            await users.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Mapping

        private static UserViewDto ToViewDto(User user)
        {
            return new UserViewDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                IsDeleted = user.IsDeleted
            };
        }

        #endregion
    }
}
