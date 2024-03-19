using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.ExpenseDetails
{
    public interface IExpenseDetailAppService : IApplicationService
    {
        Task<ExpenseDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<ExpenseDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<ExpenseDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<ExpenseDetailReadDto>> GetListByExpenseAsync(Guid projectOrderId);


        Task<ExpenseDetailReadDto> CreateAsync(ExpenseDetailCreateDto input);

        Task UpdateAsync(Guid id, ExpenseDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<ExpenseLookupDto>> GetExpenseLookupAsync();
    }
}
