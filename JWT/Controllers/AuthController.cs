using System.Threading.Tasks;
using JWT.DTOs;
using JWT.Services;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message ?? "Registration failed.");

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDto);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message ?? "Login failed.");

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] RevokeTokenDto model)
        {
            var token = model.RefreshToken ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
                return BadRequest("Refresh token is missing.");

            var result = await _authService.RevokeTokenAsync(token);
            
            if (!result)
                return BadRequest("Failed to LogOut.");

            Response.Cookies.Delete("refreshToken");

            return Ok(new { message = "LogOut successfully." });
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(new { error = result });

            return Ok(new { message = "Role added successfully.", model });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDto model)
        {
            var RefreshToken = model.RefreshToken ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(RefreshToken))
                return BadRequest("Refresh token is missing.");

            var result = await _authService.RefreshTokenAsync(RefreshToken);

            if (result is null || !result.IsAuthenticated)
                return BadRequest(result?.Message ?? "Token refresh failed.");

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordDto model)
        {
            var result = await _authService.ForgotPasswordAsync(model);
            if (!result)
                return BadRequest("Failed to send password reset email.");

            return Ok(new { message = "Password reset email sent successfully." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromQuery] string userId, [FromQuery] string token, [FromBody] string NewPassword)
        {
            
            var result = await _authService.ResetPasswordAsync(userId,token,NewPassword);
            if (!result)
                return BadRequest("Failed to reset password.");
            return Ok(new { message = "Password reset successfully." });
        }


        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime()
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
