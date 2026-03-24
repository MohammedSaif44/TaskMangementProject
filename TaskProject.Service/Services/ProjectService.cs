using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.Project;
using TaskProject.Core.Dtos.Task;
using TaskProject.Core.Entites;
using TaskProject.Core.Entites.Identity;
using TaskProject.Core.PaginationResponses;
using TaskProject.Core.Repository.Contract;
using TaskProject.Core.Services.Contract;
using TaskProject.Core.Specification;
using TaskProject.Repository.Specification;
using TaskProject.Repository.UnitOfWork;

namespace TaskProject.Service.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public ProjectService(IUnitOfWork _unitOfWork, IMapper _mapper, UserManager<AppUser> userManager)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
            this.userManager = userManager;
        }
        public async Task<bool> DeleteProjectAsync(int id)
        {
            var repo = unitOfWork.Repositories<Project, int>();

            var project = await repo.GetByIdAsync(id);

            if (project == null)
                return false;

            repo.DeleteAsync(project);
            await unitOfWork.completeAsync();

            return true;
        }

        //public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        //{
        //    return mapper.Map<IEnumerable<ProjectDto>>(await unitOfWork.Repositories<Project, int>().GetAllAsync());
        //}

        public async Task<ProjectDto?> GetProjectByIdAsync(int id)
        {
            var project = await unitOfWork.Repositories<Project, int>().GetByIdAsync(id);

            if (project == null)
                return null;

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                OwnerId = project.OwnerId,
                OwnerUsername = project.Owner?.Username,
                MembersCount = project.Members?.Count ?? 0,
                TasksCount = project.Tasks?.Count ?? 0
            };
        }

        public async Task<ProjectDto?> CreateProjectAsync(ProjectCreateDto model, int userId)
        {
            var projectRepo = unitOfWork.Repositories<Project, int>();
            var userRepo = unitOfWork.Repositories<User, int>();

            var user = await userRepo.GetByIdAsync(userId);

            if (user == null)
                return null;

            var project = mapper.Map<Project>(model);

            project.OwnerId = userId;

            await projectRepo.AddAsync(project);
            await unitOfWork.completeAsync();

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                OwnerId = user.Id,
                OwnerUsername = user.Username,
                MembersCount = 0,
                TasksCount = 0
            };
        }
       

        public async Task<bool> AddMemberAsync(int projectId, int userId)
        {
            var users =  unitOfWork.Repositories<User, int>();
            var projects =  unitOfWork.Repositories<Project, int>();
            var userres=await users.GetByIdAsync(userId);
            
            if (userres == null)
                return false;
            var projectres = await projects.GetByIdAsync(projectId);
            if (projectres == null)
                return false;
            projectres.Members.Add(userres);
            await unitOfWork.completeAsync();
            return true;


        }

        public async Task<PaginationResponse<ProjectDto>> GetAllProjectAsync(ProjectSpecParams param)
        {
            var spec = new ProjectSpecification(param);

            var projects = await unitOfWork
                .Repositories<Project, int>()
                .GetAllWithSpecAsync(spec);
            
            var mapping = mapper.Map<IEnumerable<ProjectDto>>(projects);
            
            return new PaginationResponse<ProjectDto>(param.PageSize, param.PageIndex, 0, mapping);

            

        }
    }
}
