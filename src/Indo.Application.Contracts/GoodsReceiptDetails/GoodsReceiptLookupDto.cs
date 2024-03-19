using System;
using Volo.Abp.Application.Dtos;

namespace Indo.GoodsReceiptDetails
{
    public class GoodsReceiptLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
