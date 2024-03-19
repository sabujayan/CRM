using System;
using Volo.Abp.Application.Dtos;

namespace Indo.VendorDebitNoteDetails
{
    public class UomLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
