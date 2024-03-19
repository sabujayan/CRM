using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.SalesOrderDetails
{
    public interface ISalesOrderDetailAppService : IApplicationService
    {
        Task<SalesOrderDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<SalesOrderDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<SalesOrderDetailReadDto>> GetListDetailAsync();

        Task<List<SalesOrderDetailReadDto>> GetListHighPerformerAsync();

        Task<List<SalesOrderDetailReadDto>> GetListLowPerformerAsync();

        Task<PagedResultDto<SalesOrderDetailReadDto>> GetListBySalesOrderAsync(Guid salesOrderId);

        Task<SalesOrderDetailReadDto> CreateAsync(SalesOrderDetailCreateDto input);

        Task UpdateAsync(Guid id, SalesOrderDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<SalesOrderLookupDto>> GetSalesOrderLookupAsync();

        Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync();
    }
}
