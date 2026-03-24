using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.Task;
using TaskProject.Core.Entites;

namespace TaskProject.Core.Mapping
{
    public class TaskProfile:Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskItem, TaskDto>()
                .ForMember(d => d.ProjectName,
                    o => o.MapFrom(s => s.Project.Name))
                .ForMember(d => d.AssignedToUsername,
                    o => o.MapFrom(s => s.AssignedTo != null ? s.AssignedTo.Username : null))
                .ForMember(d => d.Status,
                    o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Priority,
                    o => o.MapFrom(s => s.Priority.ToString()));
            CreateMap<TaskCreateDto, TaskItem>();
        }
    }
}
