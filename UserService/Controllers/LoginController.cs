using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using User.Application.Services;
using User.Domain.Exceptions;
using User.Domain.Models.Requests;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        protected ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }


        [HttpPost]
        public IActionResult Login([FromBody] User.Domain.Models.Requests.LoginRequest request)
        {
            try
            {
                var token = _loginService.Login(request.Username, request.Password);
                return Ok(new { token });
            }
            catch (InvalidCredentialsException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminPage()
        {
            return Ok("Dane tylko dla administratora");
        }
    }
}
