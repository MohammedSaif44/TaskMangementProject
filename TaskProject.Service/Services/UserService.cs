using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.User;
using TaskProject.Core.Entites;
using TaskProject.Core.Repository.Contract;
using TaskProject.Core.Services.Contract;

namespace TaskProject.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
            _passwordHasher = new PasswordHasher<User>();
        }
        public async Task<UserDto?> CreateUserAsync(UserCreateDto model)
        {
            var repo = unitOfWork.Repositories<User, int>();

            var user = mapper.Map<User>(model);
            user.PasswordHash =
          _passwordHasher.HashPassword(user, model.Password);

            // user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            await repo.AddAsync(user);
            await unitOfWork.completeAsync();

            return mapper.Map<UserDto>(user);
           
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var repo =unitOfWork.Repositories<User, int>();
            var user = await repo.GetByIdAsync(id);
            if (user == null)
                return false;
            repo.DeleteAsync(user);
            return true;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await unitOfWork.Repositories<User, int>().GetAllAsync();
            return mapper.Map<IEnumerable<UserDto>>(users);
            
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user=await unitOfWork.Repositories<User, int>().GetByIdAsync(id);
            if (user == null)
                return null;
            return mapper.Map<UserDto>(user);
            
        }
    }
}
