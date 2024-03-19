using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Expenses
{
    public class CustomerLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
