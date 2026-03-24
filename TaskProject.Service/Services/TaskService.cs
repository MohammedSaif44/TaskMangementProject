using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.Analytics;
using TaskProject.Core.Dtos.Task;
using TaskProject.Core.Entites;
using TaskProject.Core.Enums;
using TaskProject.Core.PaginationResponses;
using TaskProject.Core.Repository.Contract;
using TaskProject.Core.Services.Contract;
using TaskProject.Core.Specification;
using TaskProject.Repository.Specification;
using TaskProject.Repository.UnitOfWork;

namespace TaskProject.Service.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TaskService(IUnitOfWork _unitOfWork,IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }
        public async Task<TaskDto?> CreateTaskAsync(TaskCreateDto model)
        {
            var projectRepo = unitOfWork.Repositories<Project, int>();
            var userRepo = unitOfWork.Repositories<User, int>();
            var taskRepo = unitOfWork.Repositories<TaskItem, int>();

            var project = await projectRepo.GetByIdAsync(model.ProjectId);
            if (project == null)
                return null;

            var user = await userRepo.GetByIdAsync(model.AssignedToId);
            if (user == null)
                return null;

            var task = mapper.Map<TaskItem>(model);

            await taskRepo.AddAsync(task);
            await unitOfWork.completeAsync();

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status.ToString(),
                Priority = task.Priority.ToString(),
                ProjectId = project.Id,
                ProjectName = project.Name,
                AssignedToId = user.Id,
                AssignedToUsername = user.Username
            };
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var repo = unitOfWork.Repositories<TaskItem, int>();

            var task = await repo.GetByIdAsync(id);

            if (task == null)
                return false;

            repo.DeleteAsync(task);

            await unitOfWork.completeAsync();

            return true;
        }

        public async Task<PaginationResponse<TaskDto>> GetAllTasksAsync(TaskSpecParams param)
        {
            var spec = new TaskSpecification(param);
            var countSpec = new TaskCountSpecification(param);

            var tasks = await unitOfWork
                .Repositories<TaskItem, int>()
                .GetAllWithSpecAsync(spec);

            var count = await unitOfWork
                .Repositories<TaskItem, int>()
                .GetCountAsync(countSpec);

            var data = mapper.Map<IEnumerable<TaskDto>>(tasks);

            return new PaginationResponse<TaskDto>(
                param.PageSize,
                param.PageIndex,
                count,
                data
            );
        }



        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            var Mapper = await unitOfWork.Repositories<TaskItem, int>().GetByIdAsync(id);
            if (Mapper == null)
                return null;
            return mapper.Map<TaskDto>(Mapper);
        }

        public async Task<bool> UpdateStatusAsync(int taskId, Taskstatus status)
        {
            var taskrepo = unitOfWork.Repositories<TaskItem, int>();
            var TaskId=await taskrepo.GetByIdAsync(taskId);
            if (TaskId == null) return false;
            TaskId.Status = status;
            await unitOfWork.completeAsync();

            return true;



        }
    }
}
