using System;
using Indo.NumberSequences;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Movements
{
    public class Movement : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public DateTime MovementDate { get; set; }
        public string SourceDocument { get; set; }
        public NumberSequenceModules Module { get; set; }
        public Guid ProductId { get; set; }
        public float Qty { get; set; }
        public Guid FromWarehouseId { get; set; }
        public Guid ToWarehouseId { get; set; }

        private Movement() { }
        internal Movement(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid fromWarehouseId,
            [NotNull] Guid toWarehouseId,
            [NotNull] DateTime movementDate,
            [NotNull] string sourceDocument,
            [NotNull] NumberSequenceModules module,
            [NotNull] Guid productId,
            [NotNull] float qty
            ) 
            : base(id)
        {
            SetName(number);
            FromWarehouseId = fromWarehouseId;
            ToWarehouseId = toWarehouseId;
            MovementDate = movementDate;
            SourceDocument = sourceDocument;
            Module = module;
            ProductId = productId;
            Qty = qty;
        }        
        internal Movement ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: MovementConsts.MaxNumberLength
                );
        }
    }
}
