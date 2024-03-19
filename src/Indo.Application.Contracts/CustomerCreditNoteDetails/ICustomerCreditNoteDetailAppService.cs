using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.CustomerCreditNoteDetails
{
    public interface ICustomerCreditNoteDetailAppService : IApplicationService
    {
        Task<CustomerCreditNoteDetailReadDto> GetAsync(Guid id);

        Task<PagedResultDto<CustomerCreditNoteDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<List<CustomerCreditNoteDetailReadDto>> GetListDetailAsync();

        Task<PagedResultDto<CustomerCreditNoteDetailReadDto>> GetListByCustomerCreditNoteAsync(Guid serviceOrderId);

        Task<CustomerCreditNoteDetailReadDto> CreateAsync(CustomerCreditNoteDetailCreateDto input);

        Task UpdateAsync(Guid id, CustomerCreditNoteDetailUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CustomerCreditNoteLookupDto>> GetCustomerCreditNoteLookupAsync();

        Task<ListResultDto<UomLookupDto>> GetUomLookupAsync();
    }
}
