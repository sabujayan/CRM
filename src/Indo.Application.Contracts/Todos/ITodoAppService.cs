using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Todos
{
    public interface ITodoAppService : IApplicationService
    {
        Task<TodoReadDto> GetAsync(Guid id);

        Task<List<TodoReadDto>> GetListAsync();

        Task<TodoReadDto> CreateAsync(TodoCreateDto input);

        Task UpdateAsync(Guid id, TodoUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
