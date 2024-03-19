using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ExpenseDetails
{
    public class ExpenseDetail : FullAuditedAggregateRoot<Guid>
    {
        public Guid ExpenseId { get; set; }
        public string SummaryNote { get; set; }
        public float Price { get; set; }

        private ExpenseDetail() { }
        internal ExpenseDetail(
            Guid id,
            [NotNull] Guid expenseId,
            [NotNull] string summaryNote,
            [NotNull] float price
            ) 
            : base(id)
        {
            ExpenseId = expenseId;
            SummaryNote = summaryNote;
            Price = price;
        }  

    }
}
