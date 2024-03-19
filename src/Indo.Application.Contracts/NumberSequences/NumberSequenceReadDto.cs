using System;
using Volo.Abp.Application.Dtos;

namespace Indo.NumberSequences
{
    public class NumberSequenceReadDto : AuditedEntityDto<Guid>
    {
        public string Suffix { get; set; }
        public NumberSequenceModules Module { get; set; }
        public Int64 NextNumber { get; set; }
    }
}
