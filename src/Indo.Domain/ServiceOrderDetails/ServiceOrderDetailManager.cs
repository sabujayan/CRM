using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Services;
using Indo.Uoms;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ServiceOrderDetails
{
    public class ServiceOrderDetailManager : DomainService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUomRepository _uomRepository;

        public ServiceOrderDetailManager(
            IServiceRepository serviceRepository,
            IUomRepository uomRepository
            )
        {
            _serviceRepository = serviceRepository;
            _uomRepository = uomRepository;
        }
        public async Task<ServiceOrderDetail> CreateAsync(
            [NotNull] Guid serviceOrderId,
            [NotNull] Guid serviceId,
            [NotNull] float quantity,
            [NotNull] float discAmt
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(serviceOrderId, nameof(serviceOrderId));
            Check.NotNull<Guid>(serviceId, nameof(serviceId));
            Check.NotNull<float>(quantity, nameof(quantity));

            var serviceOrderDetail = new ServiceOrderDetail(
                GuidGenerator.Create(),
                serviceOrderId,
                serviceId,
                quantity,
                discAmt
            );

            var service = _serviceRepository.Where(x => x.Id.Equals(serviceId)).FirstOrDefault();
            
            serviceOrderDetail.Price = service.Price;
            serviceOrderDetail.TaxRate = service.TaxRate;
            serviceOrderDetail.Recalculate();

            var uom = _uomRepository.Where(x => x.Id.Equals(service.UomId)).FirstOrDefault();
            serviceOrderDetail.UomName = uom.Name;

            return serviceOrderDetail;
        }
    }
}
