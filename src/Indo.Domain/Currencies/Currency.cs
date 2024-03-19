using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Currencies
{
    public class Currency : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Symbol { get; set; }

        private Currency() { }
        internal Currency(
            Guid id,
            [NotNull] string name,
            [NotNull] string symbol
            ) 
            : base(id)
        {
            SetName(name);
            Symbol = symbol;
        }        
        internal Currency ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: CurrencyConsts.MaxNameLength
                );
        }
    }
}
