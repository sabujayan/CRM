using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Customers
{
    public interface ICustomerAppService : IApplicationService
    {
        Task<CustomerReadDto> GetAsync(Guid id);

        Task<List<CustomerReadDto>> GetListAsync();

        Task<List<CustomerReadDto>> GetListLeadAsync();

        Task<List<CustomerReadDto>> GetListCustomerAsync();

        Task<List<CustomerReadDto>> GetListByCustomerAsync(Guid customerId);

        Task<CustomerReadDto> CreateAsync(CustomerCreateDto input);

        Task UpdateAsync(Guid id, CustomerUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<CustomerReadDto> ConvertLeadToCustomerAsync(Guid customerId);

        Task<ListResultDto<LeadRatingLookupDto>> GetLeadRatingLookupAsync();

        Task<ListResultDto<LeadSourceLookupDto>> GetLeadSourceLookupAsync();
        Task<int> GetLeadCountByStage(CustomerStage stage);
    }
}
