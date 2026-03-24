using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Entites.Identity;
using TaskProject.Core.Services.Contract;

namespace TaskProject.Service.Services
{
    public class TokenService:ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"])
            );

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var authClaims = new List<Claim>()
               {
     new Claim(JwtRegisteredClaimNames.Sub, user.Id),
    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
    new Claim(ClaimTypes.GivenName, user.UserName ?? string.Empty)
               };

            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                authClaims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
            }

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(

                issuer: configuration["Jwt:Issuer"],

                audience: configuration["Jwt:Audience"],

                claims: authClaims,

                expires: DateTime.Now.AddDays(
                    double.Parse(configuration["Jwt:DurationInDays"])
                ),

                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
