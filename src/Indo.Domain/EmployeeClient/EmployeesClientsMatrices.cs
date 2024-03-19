using Indo.Employees;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.EmployeeClient
{
    public class EmployeesClientsMatrices : FullAuditedAggregateRoot<Guid>
    {
        public Guid EmployeeId { get; set; }
        public Guid ClientsId { get; set; }
        private EmployeesClientsMatrices() { }
        internal EmployeesClientsMatrices(
             Guid id,
             [NotNull] Guid employeeid,
             [NotNull] Guid clientsid
             )
             : base(id)
        {

            EmployeeId = employeeid;
            ClientsId = clientsid;
        }

    }
}
