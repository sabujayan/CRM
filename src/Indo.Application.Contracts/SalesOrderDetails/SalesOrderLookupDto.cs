using System;
using Volo.Abp.Application.Dtos;

namespace Indo.SalesOrderDetails
{
    public class SalesOrderLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
