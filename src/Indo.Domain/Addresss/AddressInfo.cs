using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Addresss
{
    public class AddressInfo : FullAuditedAggregateRoot<Guid>
    {
        [AllowNull]
        public string Street { get; set; }
        [AllowNull]
        public string State { get; set; }
        [AllowNull]
        public string Country { get; set; }
        [AllowNull]
        public string City { get; set; }
        [AllowNull]
        public string ZipCode { get; set; }
        public AddressInfo() { }

        public AddressInfo(Guid id, string street, string state, string country, string city, string zipCode)
            : base(id)
        {
            Street = street;
            State = state;
            Country = country;
            City = city;
            ZipCode = zipCode;
        }
    }
}
