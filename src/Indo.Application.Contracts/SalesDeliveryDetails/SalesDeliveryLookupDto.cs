using System;
using Volo.Abp.Application.Dtos;

namespace Indo.SalesDeliveryDetails
{
    public class SalesDeliveryLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
