using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.ExpenseTypes
{
    public interface IExpenseTypeAppService : IApplicationService
    {
        Task<ExpenseTypeReadDto> GetAsync(Guid id);

        Task<List<ExpenseTypeReadDto>> GetListAsync();

        Task<ExpenseTypeReadDto> CreateAsync(ExpenseTypeCreateDto input);

        Task UpdateAsync(Guid id, ExpenseTypeUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
