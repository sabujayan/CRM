using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Expenses
{
    public class EmployeeLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
