using Indo.Address;
using System.Threading.Tasks;
using System;
using Volo.Abp.Application.Services;
using System.Collections.Generic;
using Indo.Clientes;

namespace Indo.Leads
{
    public interface ILeadsAppService : IApplicationService
    {
        Task<LeadWithAddressesDto> GetLeadWithAddressesAsync(Guid id);
        Task<Guid> CreateLeadWithAddressesAsync(CreateLeadDto input);
        Task UpdateLeadAsync(Guid id, UpdateLeadDto input);
        Task DeleteLeadAsync(Guid id);
    }

}
