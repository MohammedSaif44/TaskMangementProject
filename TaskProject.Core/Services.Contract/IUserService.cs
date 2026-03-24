using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.User;

namespace TaskProject.Core.Services.Contract
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> CreateUserAsync(UserCreateDto model);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<bool> DeleteUserAsync(int id);

    }
}
