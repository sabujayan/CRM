using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.VendorBillDetails
{
    public interface IVendorBillDetailAppService : IApplicationService
    {
        Task<VendorBillDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<VendorBillDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<VendorBillDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<VendorBillDetailReadDto>> GetListByVendorBillAsync(Guid serviceOrderId);

        Task<VendorBillDetailReadDto> CreateAsync(VendorBillDetailCreateDto input);

        Task UpdateAsync(Guid id, VendorBillDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<VendorBillLookupDto>> GetVendorBillLookupAsync();

        Task<ListResultDto<UomLookupDto>> GetUomLookupAsync();
    }
}
