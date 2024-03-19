using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Tasks
{
    public class TaskManager : DomainService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskManager(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task<Task> CreateAsync(
            [NotNull] string name,
            [NotNull] DateTime startTime,
            [NotNull] DateTime endTime,
            [NotNull] Guid customerId,
            [NotNull] Guid activityId
            )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(startTime, nameof(startTime));
            Check.NotNull(endTime, nameof(endTime));
            Check.NotNull(customerId, nameof(customerId));
            Check.NotNull(activityId, nameof(activityId));

            var existing = await _taskRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new TaskAlreadyExistsException(name);
            }

            return new Task(
                GuidGenerator.Create(),
                name,
                startTime,
                endTime,
                customerId,
                activityId
            );
        }
        public async System.Threading.Tasks.Task ChangeNameAsync(
           [NotNull] Task task,
           [NotNull] string newName
            )
        {
            Check.NotNull(task, nameof(task));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _taskRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != task.Id)
            {
                throw new TaskAlreadyExistsException(newName);
            }

            task.ChangeName(newName);
        }
    }
}
