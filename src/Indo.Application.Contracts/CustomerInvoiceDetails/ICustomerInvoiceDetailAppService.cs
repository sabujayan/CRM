using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.CustomerInvoiceDetails
{
    public interface ICustomerInvoiceDetailAppService : IApplicationService
    {
        Task<CustomerInvoiceDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<CustomerInvoiceDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<CustomerInvoiceDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<CustomerInvoiceDetailReadDto>> GetListByCustomerInvoiceAsync(Guid serviceOrderId);

        Task<CustomerInvoiceDetailReadDto> CreateAsync(CustomerInvoiceDetailCreateDto input);

        Task UpdateAsync(Guid id, CustomerInvoiceDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CustomerInvoiceLookupDto>> GetCustomerInvoiceLookupAsync();

        Task<ListResultDto<UomLookupDto>> GetUomLookupAsync();
    }
}
