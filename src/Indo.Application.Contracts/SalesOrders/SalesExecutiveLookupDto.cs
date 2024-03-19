using System;
using Volo.Abp.Application.Dtos;

namespace Indo.SalesOrders
{
    public class SalesExecutiveLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
