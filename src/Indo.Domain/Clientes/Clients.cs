using Indo.Departments;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Clientes
{
    public class Clients : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public  Guid? UserId { get; set; }

        private Clients() { }
        internal Clients(
            Guid id,
            [NotNull] string name
            )
            : base(id)
        {
            SetName(name);
        }
        internal Clients ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ClientConsts.MaxNameLength
                );
        }
    }
}
