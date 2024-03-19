using Indo.Projectes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ContactsAddress
{
    public class ContactsAddressMatrix : FullAuditedAggregateRoot<Guid>
    {
        public Guid ContactId { get; set; }
        public Guid AddressId { get; set; }
        private ContactsAddressMatrix() { }
        internal ContactsAddressMatrix(
             Guid id,
             [NotNull] Guid contactId,
             [NotNull] Guid addressId
             )
             : base(id)
        {

            ContactId = contactId;
            AddressId = addressId;
        }
    }
}
