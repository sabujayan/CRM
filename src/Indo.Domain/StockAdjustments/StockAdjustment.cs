using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.StockAdjustments
{
    public class StockAdjustment : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public StockAdjustmentStatus Status { get; set; }
        public StockAdjustmentType AdjustmentType { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid FromWarehouseId { get; set; }
        public Guid ToWarehouseId { get; set; }
        public Guid ReturnId { get; set; }
        public Guid OriginalId { get; set; }

        private StockAdjustment() { }
        internal StockAdjustment(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid warehouseId,
            [NotNull] StockAdjustmentType adjustmentType,
            [NotNull] DateTime adjustmentDate
            ) 
            : base(id)
        {
            SetName(number);
            WarehouseId = warehouseId;
            AdjustmentType = adjustmentType;
            AdjustmentDate = adjustmentDate;
            Status = StockAdjustmentStatus.Draft;
        }        
        internal StockAdjustment ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: StockAdjustmentConsts.MaxNumberLength
                );
        }
    }
}
