using Indo.LeadsAddress;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Leads
{
    public class LeadsInfo : FullAuditedAggregateRoot<Guid>
    {
        public string FirstName { get; set; }
        [AllowNull]
        public string Phone { get; set; }
        [AllowNull]
        public string Industry { get; set; }
        [AllowNull]
        public string Title { get; set; }
        [AllowNull]
        public string Email { get; set; }
        public string Company { get; set; }
        public string LastName { get; set; }
        [AllowNull]
        public string Website { get; set; }
        [AllowNull]
        public string SkypeId { get; set; }
        [AllowNull]
        public string Status { get; set; }
        [AllowNull]
        public string Comments { get; set; }
        public ICollection<LeadsAddressMatrix> LeadsAddresses { get; set; }
        public LeadsInfo() { }

        public LeadsInfo(
            Guid id,
            [NotNull] string firstName,
            [NotNull] string lastName
            )
            : base(id)
        {
            SetName(firstName, lastName);
        }

        public LeadsInfo(Guid id, string firstName) : base(id)
        {
        }

        public LeadsInfo ChangeName([NotNull] string firstName, [NotNull] string lastName)
        {
            SetName(firstName, lastName);
            return this;
        }

        public void SetName([NotNull] string firstName, [NotNull] string lastName)
        {
            FirstName = Check.NotNullOrWhiteSpace(
                firstName,
                nameof(firstName),
                maxLength: LeadsConsts.MaxNameLength
            );

            LastName = Check.NotNullOrWhiteSpace(
                lastName,
                nameof(lastName),
                maxLength: LeadsConsts.MaxNameLength
            );
        }
    }
}
