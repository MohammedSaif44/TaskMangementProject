using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.Auth;

namespace TaskProject.Core.Services.Contract
{
    public interface IUserAccountService
    {
        public Task<UserAuthDto> LoginAsync(LoginDto loginDto);
        public Task<UserAuthDto> RegisterAsync(RegisterDto registerDto);
        public Task<bool> CheckEmailExistAsync(string email);
        Task<bool> ForgotPasswordAsync(string email);

        Task<bool> ResetPasswordAsync(ResetPasswordDto model);
    }
}
