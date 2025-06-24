using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.Application.Services;
using User.Domain.Models.Requests;
using User.Domain.Models.Response;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<UserResponseDTO> GetUserData()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            try
            {
                var userDto = _userService.GetUser(userId);
                return Ok(userDto);
            }
            catch
            {
                return NotFound();
            }

        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterUserDTO dto)
        {
            try
            {
                _userService.RegisterUser(dto);
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            _userService.ResetPassword(dto);
            return Ok(new { message = "Password has been reset successfully." });
        }

        [Authorize]
        [HttpPut("edit-profile")]
        public IActionResult EditProfile([FromBody] EditUserDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            _userService.EditUser(userId, dto);
            return Ok(new { message = "Profile updated successfully." });
        }
    }
}
