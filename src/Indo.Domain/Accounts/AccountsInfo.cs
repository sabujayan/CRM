using Indo.Contacts;
using Indo.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Accounts
{
    public class AccountsInfo : FullAuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; set; }

        [AllowNull]
        public byte[] AccountImage { get; set; }
        public string AccountName { get; set; }
        [AllowNull]
        public string AccountSite { get; set; }
        [AllowNull]
        public string ParentAccount { get; set; }
        [AllowNull]
        public string AccountNumber { get; set; }
        [AllowNull]
        public string AccountType { get; set; }
        [AllowNull]
        public string Industry { get; set; }
        [AllowNull]
        public decimal AnnualRevenue { get; set; }
        [AllowNull]
        public int Rating { get; set; }
        [AllowNull]
        public string Phone { get; set; }
        [AllowNull]
        public string Fax { get; set; }
        [AllowNull]
        public string Website { get; set; }
        [AllowNull]
        public string TickerSymbol { get; set; }
        [AllowNull]
        public string Ownership { get; set; }
        [AllowNull]
        public int Employees { get; set; }
        [AllowNull]
        public string SICCode { get; set; }
        [AllowNull]
        public string BillingStreet { get; set; }
        [AllowNull]
        public string BillingCity { get; set; }
        [AllowNull]
        public string BillingState { get; set; }
        [AllowNull]
        public string BillingCode { get; set; }
        [AllowNull]
        public string BillingCountry { get; set; }
        [AllowNull]
        public string ShippingStreet { get; set; }
        [AllowNull]
        public string ShippingCity { get; set; }
        [AllowNull]
        public string ShippingState { get; set; }
        [AllowNull]
        public string ShippingCode { get; set; }
        [AllowNull]
        public string ShippingCountry { get; set; }
        [AllowNull]
        public string Description { get; set; }

        private AccountsInfo() { }

        internal AccountsInfo(
            Guid id,
            [NotNull] string accountName
            )
            : base(id)
        {
            SetName(accountName);
        }

        internal AccountsInfo ChangeName([NotNull] string accountName)
        {
            SetName(accountName);
            return this;
        }

        private void SetName([NotNull] string accountName)
        {
            AccountName = Check.NotNullOrWhiteSpace(
                accountName,
                nameof(accountName),
                maxLength: AccountConsts.MaxNameLength
            );
        }
    }
}
