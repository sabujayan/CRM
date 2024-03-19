using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.TransferOrderDetails
{
    public interface ITransferOrderDetailAppService : IApplicationService
    {
        Task<TransferOrderDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<TransferOrderDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<PagedResultDto<TransferOrderDetailReadDto>> GetListByTransferOrderAsync(Guid transferOrderId);

        Task<TransferOrderDetailReadDto> CreateAsync(TransferOrderDetailCreateDto input);

        Task UpdateAsync(Guid id, TransferOrderDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<TransferOrderLookupDto>> GetTransferOrderLookupAsync();

        Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync();
    }
}
