using System;
using Volo.Abp.Application.Dtos;

namespace Indo.VendorDebitNoteDetails
{
    public class VendorDebitNoteLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
