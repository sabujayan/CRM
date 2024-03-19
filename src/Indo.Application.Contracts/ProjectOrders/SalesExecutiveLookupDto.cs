using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ProjectOrders
{
    public class SalesExecutiveLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
