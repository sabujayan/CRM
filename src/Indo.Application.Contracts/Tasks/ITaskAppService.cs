using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Tasks
{
    public interface ITaskAppService : IApplicationService
    {
        Task<TaskReadDto> GetAsync(Guid id);

        Task<List<TaskReadDto>> GetListAsync();

        Task<List<TaskReadDto>> GetListByCustomerAsync(Guid customerId);

        Task<TaskReadDto> CreateAsync(TaskCreateDto input);

        Task UpdateAsync(Guid id, TaskUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();

        Task<ListResultDto<ActivityLookupDto>> GetActivityLookupAsync();
    }
}
