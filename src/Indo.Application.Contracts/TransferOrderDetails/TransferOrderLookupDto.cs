using System;
using Volo.Abp.Application.Dtos;

namespace Indo.TransferOrderDetails
{
    public class TransferOrderLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
