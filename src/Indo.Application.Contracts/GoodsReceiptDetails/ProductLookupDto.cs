using System;
using Volo.Abp.Application.Dtos;

namespace Indo.GoodsReceiptDetails
{
    public class ProductLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
