using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Expenses
{
    public interface IExpenseAppService : IApplicationService
    {
        Task<ExpenseReadDto> GetAsync(Guid id);

        Task<float> GetSummaryTotalAsync(Guid id);

        Task<string> GetSummaryTotalInStringAsync(Guid id);

        Task<List<ExpenseReadDto>> GetListAsync();

        Task<List<ExpenseReadDto>> GetListByCustomerAsync(Guid customerId);

        Task<ExpenseReadDto> CreateAsync(ExpenseCreateDto input);

        Task UpdateAsync(Guid id, ExpenseUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<ExpenseTypeLookupDto>> GetExpenseTypeLookupAsync();

        Task<ListResultDto<EmployeeLookupDto>> GetEmployeeLookupAsync();

        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();
    }
}
