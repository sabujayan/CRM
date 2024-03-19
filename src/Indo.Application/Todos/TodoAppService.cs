using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.Todos
{
    public class TodoAppService : IndoAppService, ITodoAppService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly TodoManager _todoManager;
        public TodoAppService(
            ITodoRepository todoRepository,
            TodoManager todoManager
            )
        {
            _todoRepository = todoRepository;
            _todoManager = todoManager;
        }
        public async Task<TodoReadDto> GetAsync(Guid id)
        {
            var obj = await _todoRepository.GetAsync(id);
            return ObjectMapper.Map<Todo, TodoReadDto>(obj);
        }
        public async Task<List<TodoReadDto>> GetListAsync()
        {
            var queryable = await _todoRepository.GetQueryableAsync();
            var query = from todo in queryable
                        select new { todo };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Todo, TodoReadDto>(x.todo);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<TodoReadDto> CreateAsync(TodoCreateDto input)
        {
            var obj = await _todoManager.CreateAsync(
                input.Name,
                input.StartTime,
                input.EndTime
            );

            obj.Description = input.Description;
            obj.IsDone = input.IsDone;
            obj.Location = input.Location;

            await _todoRepository.InsertAsync(obj);

            return ObjectMapper.Map<Todo, TodoReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, TodoUpdateDto input)
        {
            var obj = await _todoRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _todoManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;
            obj.StartTime = input.StartTime;
            obj.EndTime = input.EndTime;
            obj.IsDone = input.IsDone;
            obj.Location = input.Location;

            await _todoRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _todoRepository.DeleteAsync(id);
        }
    }
}
