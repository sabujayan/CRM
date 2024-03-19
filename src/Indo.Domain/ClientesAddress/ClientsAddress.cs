using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ClientesAddress
{
    public class ClientsAddress : FullAuditedAggregateRoot<Guid>
    {
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public Guid ClientsId { get; set; }

        private ClientsAddress() { }
        internal ClientsAddress(
            Guid id,
            [NotNull] string address,
            [NotNull] Guid clientsid,
            [NotNull] string country,
            [NotNull] string state,
            [NotNull] string city,
            [NotNull] string zip
            )
            : base(id)
        {
            SetName(address);
            ClientsId = clientsid;
            Country = country;
            State = state;
            City = city;
            Zip = zip;
        }
        internal ClientsAddress ChangeName([NotNull] string address)
        {
            SetName(address);
            return this;
        }
        private void SetName([NotNull] string address)
        {
            Address = Check.NotNullOrWhiteSpace(
                address,
                nameof(address),
                maxLength: ClientsAddressConsts.MaxNameLength
                );
        }
    }
}
