using System;
using Volo.Abp.Application.Dtos;

namespace Indo.CustomerCreditNoteDetails
{
    public class UomLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
