using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ServiceOrderDetails
{
    public class ServiceOrderLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
