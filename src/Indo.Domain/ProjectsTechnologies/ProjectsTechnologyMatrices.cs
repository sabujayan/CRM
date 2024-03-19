using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ProjectsTechnologies
{
    public class ProjectsTechnologyMatrices : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProjectsId { get; set; }
        public Guid TechnologyId { get; set; }
        private ProjectsTechnologyMatrices() { }
        internal ProjectsTechnologyMatrices(
             Guid id,
             [NotNull] Guid projectsId,
             [NotNull] Guid technologyid
             )
             : base(id)
        {

            ProjectsId = projectsId;
            TechnologyId = technologyid;
        }
    }
}
