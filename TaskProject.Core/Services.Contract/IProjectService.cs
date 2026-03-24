using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.Project;
using TaskProject.Core.Entites;
using TaskProject.Core.PaginationResponses;
using TaskProject.Core.Specification;

namespace TaskProject.Core.Services.Contract
{
    public interface IProjectService
    {
        Task<PaginationResponse<ProjectDto>> GetAllProjectAsync(ProjectSpecParams productSpecParams);
        Task<ProjectDto?> GetProjectByIdAsync(int id);
        Task<ProjectDto?> CreateProjectAsync(ProjectCreateDto model, int userId);
        Task<bool> DeleteProjectAsync(int id);
        Task<bool> AddMemberAsync(int projectId, int userId);

       


    }
}
