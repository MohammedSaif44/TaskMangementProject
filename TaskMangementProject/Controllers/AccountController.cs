using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskProject.Core.Dtos.Auth;
using TaskProject.Core.Entites.Identity;
using TaskProject.Core.Services.Contract;
using TaskProject.Service.Services;

namespace TaskMangementProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserAccountService userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(IUserAccountService userService, UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            this.userService = userService;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserAuthDto>> Register(RegisterDto model)
        {
            var user = await userService.RegisterAsync(model);

            if (user == null)
                return BadRequest("Invalid Registration");

            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserAuthDto>> Login(LoginDto model)
        {
            var user = await userService.LoginAsync(model);

            if (user == null)
                return Unauthorized();

            return Ok(user);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordDto model)
        {
            var result = await userService.ForgotPasswordAsync(model.Email);

            if (!result)
                return BadRequest("User Not Found");

            return Ok("Reset token sent");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var result = await userService.ResetPasswordAsync(model);

            if (!result)
                return BadRequest("Reset Failed");

            return Ok("Password Reset Successfully");
        }
    }
}
