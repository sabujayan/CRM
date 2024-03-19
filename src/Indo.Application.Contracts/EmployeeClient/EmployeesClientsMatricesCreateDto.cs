using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.EmployeeClient
{
    public class EmployeesClientsMatricesCreateDto : FullAuditedAggregateRoot<Guid>
    {
        public Guid EmployeeId { get; set; }
        public Guid ClientsId { get; set; }
    }
}
