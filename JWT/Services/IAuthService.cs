using JWT.DTOs;

namespace JWT.Services
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthDto> LoginAsync(LoginDto loginDto);
        Task<string> AddRoleAsync(AddRoleDto model);
        Task<AuthDto> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
        Task<bool> ForgotPasswordAsync(ForgotPasswordDto model);
        Task<bool> ResetPasswordAsync(string userId,string token,string NewPassword);
    }
}