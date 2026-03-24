using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.Auth;
using TaskProject.Core.Entites.Identity;
using TaskProject.Core.Services.Contract;

namespace TaskProject.Service.Services.Users
{
    public class UserAccountService:IUserAccountService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;

        public UserAccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
        }

        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await userManager.FindByEmailAsync(email) is not null;
        }

        public async Task<UserAuthDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return null;

            var result = await signInManager.CheckPasswordSignInAsync(
                user,loginDto.Password,  false
            );

            if (!result.Succeeded)
                return null;

            return new UserAuthDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await tokenService.CreateTokenAsync(user, userManager)
            };
        }

        public async Task<UserAuthDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await CheckEmailExistAsync(registerDto.Email))
                return null;

            var user = new AppUser()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Email.Split("@")[0]
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return null;

            return new UserAuthDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await tokenService.CreateTokenAsync(user, userManager)
            };
        }
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
                return false;

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            // هنا المفروض تبعت ايميل
            Console.WriteLine($"Reset Token: {token}");

            return true;
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return false;

            var result = await userManager.ResetPasswordAsync(
                user,
                model.Token,
                model.NewPassword
            );

            return result.Succeeded;
        }
    }
}
