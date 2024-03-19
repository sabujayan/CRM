using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Contacts
{
    public interface IContactAppService : IApplicationService
    {
        Task<ContactReadDto> GetAsync(Guid id);

        Task<List<ContactReadDto>> GetListAsync();

        Task<List<ContactReadDto>> GetListByCustomerAsync(Guid customerId);

        Task<ContactReadDto> CreateAsync(ContactCreateDto input);

        Task UpdateAsync(Guid id, ContactUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();
    }
}
