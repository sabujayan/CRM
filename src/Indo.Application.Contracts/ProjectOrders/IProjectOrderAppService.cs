using Indo.CustomerInvoices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.ProjectOrders
{
    public interface IProjectOrderAppService : IApplicationService
    {
        Task<ProjectOrderReadDto> GetAsync(Guid id);

        Task<float> GetSummaryTotalAsync(Guid id);

        Task<string> GetSummaryTotalInStringAsync(Guid id);

        Task<float> GetSummaryTotalByYearMonthAsync(int year, int month);

        Task<OrderCountDto> GetCountOrderAsync();

        Task<List<ProjectOrderReadDto>> GetListAsync();

        Task<List<ProjectOrderReadDto>> GetListWithTotalAsync();

        Task<List<ProjectOrderReadDto>> GetListWithTotalByCustomerAsync(Guid customerId);

        Task<List<ProjectOrderReadDto>> GetListLastFiveOrderAsync();

        Task<List<MonthlyEarningDto>> GetListConfirmMonthlyEarning(int monthsCount);

        Task<List<MonthlyEarningDto>> GetListConfirmAndCancelledMonthlyEarning(int monthsCount);

        Task<ProjectOrderReadDto> CreateAsync(ProjectOrderCreateDto input);

        Task UpdateAsync(Guid id, ProjectOrderUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();

        Task<ListResultDto<SalesExecutiveLookupDto>> GetSalesExecutiveLookupAsync();

        Task<ListResultDto<RatingLookupDto>> GetRatingLookupAsync();

        Task<ProjectOrderReadDto> ConfirmAsync(Guid projectOrderId);

        Task<ProjectOrderReadDto> CancelAsync(Guid projectOrderId);

        Task<CustomerInvoiceReadDto> GenerateInvoiceAsync(Guid projectOrderId);
    }
}
