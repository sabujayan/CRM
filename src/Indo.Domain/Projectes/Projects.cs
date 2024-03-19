using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Projectes
{
    public class Projects : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public Guid ClientsId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float EstimateHours { get; set; }
        public string Notes { get; set; }
        public string Technology { get; set; }
        private Projects() { }
        internal Projects(
            Guid id,
            [NotNull] string name
            )
            : base(id)
        {
            SetName(name);
        }
        internal Projects ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ProjectsConsts.MaxNameLength
                );
        }
    }
}
