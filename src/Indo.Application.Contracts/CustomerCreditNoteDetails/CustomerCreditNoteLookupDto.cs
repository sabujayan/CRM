using System;
using Volo.Abp.Application.Dtos;

namespace Indo.CustomerCreditNoteDetails
{
    public class CustomerCreditNoteLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
