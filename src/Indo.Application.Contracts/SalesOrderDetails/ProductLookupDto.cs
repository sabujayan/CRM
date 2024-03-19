using System;
using Volo.Abp.Application.Dtos;

namespace Indo.SalesOrderDetails
{
    public class ProductLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public float RetailPrice { get; set; }
    }
}
