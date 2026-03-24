using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.Project;
using TaskProject.Core.Entites;

namespace TaskProject.Core.Mapping
{
    public class ProjectProfile:Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectDto>().ForMember(d => d.OwnerUsername, o => o.MapFrom(s => s.Owner.Username))
            .ForMember(d => d.MembersCount, o => o.MapFrom(s => s.Members.Count))
            .ForMember(d => d.TasksCount, o => o.MapFrom(s => s.Tasks.Count));


            CreateMap<ProjectCreateDto, Project>();


        }
    }
}
