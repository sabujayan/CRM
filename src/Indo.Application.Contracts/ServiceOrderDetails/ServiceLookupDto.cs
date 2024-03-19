using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ServiceOrderDetails
{
    public class ServiceLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public float Price { get; set; }
    }
}
