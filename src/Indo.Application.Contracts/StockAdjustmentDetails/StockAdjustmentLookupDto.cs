using System;
using Volo.Abp.Application.Dtos;

namespace Indo.StockAdjustmentDetails
{
    public class StockAdjustmentLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
