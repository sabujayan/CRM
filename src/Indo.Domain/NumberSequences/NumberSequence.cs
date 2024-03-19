using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.NumberSequences
{
    public class NumberSequence : FullAuditedAggregateRoot<Guid>
    {
        public string Suffix { get; set; }
        public NumberSequenceModules Module { get; set; }
        public Int64 NextNumber { get; set; }

        private NumberSequence() { }
        internal NumberSequence(
            Guid id,
            [NotNull] string suffix
            ) 
            : base(id)
        {
            SetSuffix(suffix);
            NextNumber = 1;
        }        
        internal NumberSequence ChangeSuffix([NotNull] string prefix)
        {
            SetSuffix(prefix);
            return this;
        }
        private void SetSuffix([NotNull] string prefix)
        {
            Suffix = Check.NotNullOrWhiteSpace(
                prefix,
                nameof(prefix),
                maxLength: NumberSequenceConsts.MaxPrefixLength
                );
        }

    }
}
