using ITPLibrary.Api.Core.Dtos.Email;
using ITPLibrary.Api.Core.Dtos.User;
using ITPLibrary.Api.Core.Services.EmailService;
using ITPLibrary.Api.Core.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ITPLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserDto newUser)
        {
            try
            {
                var registeredUsers = await _userService.RegisterUser(newUser);
                return CreatedAtAction(nameof(Register), registeredUsers);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<UserTokenDto>> Login([FromBody] UserLoginDto loginDto)
        {
            var response = await _userService.LoginUser(loginDto);
            
            if (response is null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(response);
        }


        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> SendEmailResetPassword(RecoverPasswordDto recoverPasswordDto)
        {
                await _userService.UpdatePasswordInDatabase(recoverPasswordDto.Email);
                return Ok("Password recovery email sent successfully.");
        }
    }
}
