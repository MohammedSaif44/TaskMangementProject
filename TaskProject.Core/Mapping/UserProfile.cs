using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.User;
using TaskProject.Core.Entites;

namespace TaskProject.Core.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.Role,
                    o => o.MapFrom(s => s.Role.ToString()));

            CreateMap<UserCreateDto, User>()
                .ForMember(d => d.PasswordHash,
                    o => o.Ignore());
        }
    }
}
