using Indo.NumberSequences;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Movements
{
    public class MovementReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public DateTime MovementDate { get; set; }
        public string SourceDocument { get; set; }
        public NumberSequenceModules Module { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string UomName { get; set; }
        public float Qty { get; set; }
        public Guid FromWarehouseId { get; set; }
        public string FromWarehouseName { get; set; }
        public Guid ToWarehouseId { get; set; }
        public string ToWarehouseName { get; set; }
    }
}
