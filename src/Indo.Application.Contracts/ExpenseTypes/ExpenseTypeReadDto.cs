using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ExpenseTypes
{
    public class ExpenseTypeReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
