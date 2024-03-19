using Indo.Addresss;
using Indo.ContactsAddress;
using Indo.Leads;
using Indo.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace Indo.Contacts
{
    public class ContactsInfo : FullAuditedAggregateRoot<Guid>
    {
        public string FirstName { get; set; }
        [AllowNull]
        public string Phone { get; set; }
        [AllowNull]
        public string Industry { get; set; }
        [AllowNull]
        public string Title { get; set; }
        [AllowNull]
        public bool Email { get; set; }
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
        public ICollection<ContactsAddressMatrix> ContactsAddresses { get; set; }
        private ContactsInfo() { }

        internal ContactsInfo(
            Guid id,
            [NotNull] string firstName,
            [NotNull] string lastName
            )
            : base(id)
        {
            SetName(firstName, lastName);
        }

        internal ContactsInfo ChangeName([NotNull] string firstName, [NotNull] string lastName)
        {
            SetName(firstName, lastName);
            return this;
        }

        private void SetName([NotNull] string firstName, [NotNull] string lastName)
        {
            FirstName = Check.NotNullOrWhiteSpace(
                firstName,
                nameof(firstName),
                maxLength: ContactConsts.MaxNameLength
            );

            LastName = Check.NotNullOrWhiteSpace(
                lastName,
                nameof(lastName),
                maxLength: ContactConsts.MaxNameLength
            );
        }
    }
}
