using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ClientsProjects
{
    public class ClientsProjectsMatrices:FullAuditedAggregateRoot<Guid>
    {
        public Guid ClientsId { get; set; }
        public Guid ProjectsId { get; set; }
        private ClientsProjectsMatrices() { }
        internal ClientsProjectsMatrices(
             Guid id,
             [NotNull] Guid clientsid,
             [NotNull] Guid projectsid
             )
             : base(id)
        {

            ClientsId = clientsid;
            ProjectsId = projectsid;
        }
    }
}
