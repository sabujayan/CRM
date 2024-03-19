using System;
using Volo.Abp.Application.Dtos;

namespace Indo.CashAndBanks
{
    public class CashAndBankReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
