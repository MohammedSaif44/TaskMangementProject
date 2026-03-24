using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaskProject.Core.Dtos.User;
using TaskProject.Core.Services.Contract;

namespace TaskMangementProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase

    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers() {
           var res=await userService.GetAllUsersAsync();
            return Ok(res);

        }
        [HttpPost]
        public async Task<ActionResult<UserDto>>CreateUser(UserCreateDto model)
        {
            var res = await userService.CreateUserAsync(model);

            if (res == null)
                return BadRequest("Invalid ProjectId or AssignedToId");
            return Ok(res);

        }

    }
}
