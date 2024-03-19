using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ClientesContact
{
    public class ClientsContact : FullAuditedAggregateRoot<Guid>
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid ClientsId { get; set; }
        private ClientsContact() { }

        internal ClientsContact(
           Guid id,
           [NotNull] string email,
           [NotNull] string phonenumber,
           [NotNull] Guid clientsid
           )
           : base(id)
        {
            SetName(email);
            PhoneNumber = phonenumber;
            ClientsId = clientsid;
        }
        internal ClientsContact ChangeName([NotNull] string email)
        {
            SetName(email);
            return this;
        }
        private void SetName([NotNull] string email)
        {
            Email = Check.NotNullOrWhiteSpace(
                email,
                nameof(email),
                maxLength: ClientsContactConsts.MaxNameLength
                );
        }
    }
}
