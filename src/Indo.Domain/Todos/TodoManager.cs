using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Todos
{
    public class TodoManager : DomainService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoManager(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }
        public async Task<Todo> CreateAsync(
            [NotNull] string name,
            [NotNull] DateTime startTime,
            [NotNull] DateTime endTime
            )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(startTime, nameof(startTime));
            Check.NotNull(endTime, nameof(endTime));

            var existing = await _todoRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new TodoAlreadyExistsException(name);
            }

            return new Todo(
                GuidGenerator.Create(),
                name,
                startTime,
                endTime
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Todo todo,
           [NotNull] string newName)
        {
            Check.NotNull(todo, nameof(todo));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _todoRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != todo.Id)
            {
                throw new TodoAlreadyExistsException(newName);
            }

            todo.ChangeName(newName);
        }
    }
}
