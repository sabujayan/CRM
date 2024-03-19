using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Indo.LeadsAddress
{
    public class LeadsAddressManager : DomainService
    {
        private readonly ILeadAddressMatrixRepository _leadsAddressMatrixRepository;

        public LeadsAddressManager(ILeadAddressMatrixRepository leadsAddressMatrixRepository)
        {
            _leadsAddressMatrixRepository = leadsAddressMatrixRepository;
        }

        public async Task<LeadsAddressMatrix> CreateAsync(Guid leadsId, Guid addressId)
        {

            var leadsAddressMatrix = new LeadsAddressMatrix(GuidGenerator.Create(), leadsId, addressId);
            return await _leadsAddressMatrixRepository.InsertAsync(leadsAddressMatrix);
        }
    }
}
