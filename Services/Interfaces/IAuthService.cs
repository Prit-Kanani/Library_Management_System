using Library_Management_System.DTOs.Auth;

namespace Library_Management_System.Services.Interfaces
{
    public interface IAuthService
    {
        #region Commands

        Task<AuthResponseDto?> LoginAsync(LoginDto dto);

        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

        #endregion
    }
}
