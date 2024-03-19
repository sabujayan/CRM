using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Services;
using Indo.Uoms;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ServiceQuotationDetails
{
    public class ServiceQuotationDetailManager : DomainService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUomRepository _uomRepository;

        public ServiceQuotationDetailManager(
            IServiceRepository serviceRepository,
            IUomRepository uomRepository
            )
        {
            _serviceRepository = serviceRepository;
            _uomRepository = uomRepository;
        }
        public async Task<ServiceQuotationDetail> CreateAsync(
            [NotNull] Guid serviceQuotationId,
            [NotNull] Guid serviceId,
            [NotNull] float quantity,
            [NotNull] float discAmt
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(serviceQuotationId, nameof(serviceQuotationId));
            Check.NotNull<Guid>(serviceId, nameof(serviceId));
            Check.NotNull<float>(quantity, nameof(quantity));

            var serviceQuotationDetail = new ServiceQuotationDetail(
                GuidGenerator.Create(),
                serviceQuotationId,
                serviceId,
                quantity,
                discAmt
            );

            var service = _serviceRepository.Where(x => x.Id.Equals(serviceId)).FirstOrDefault();
            
            serviceQuotationDetail.Price = service.Price;
            serviceQuotationDetail.TaxRate = service.TaxRate;
            serviceQuotationDetail.Recalculate();

            var uom = _uomRepository.Where(x => x.Id.Equals(service.UomId)).FirstOrDefault();
            serviceQuotationDetail.UomName = uom.Name;

            return serviceQuotationDetail;
        }
    }
}
