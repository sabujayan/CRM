using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.VendorDebitNoteDetails
{
    public interface IVendorDebitNoteDetailAppService : IApplicationService
    {
        Task<VendorDebitNoteDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<VendorDebitNoteDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<VendorDebitNoteDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<VendorDebitNoteDetailReadDto>> GetListByVendorDebitNoteAsync(Guid serviceOrderId);

        Task<VendorDebitNoteDetailReadDto> CreateAsync(VendorDebitNoteDetailCreateDto input);

        Task UpdateAsync(Guid id, VendorDebitNoteDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<VendorDebitNoteLookupDto>> GetVendorDebitNoteLookupAsync();

        Task<ListResultDto<UomLookupDto>> GetUomLookupAsync();
    }
}
