using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Expenses
{
    public class Expense : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ExpenseTypeId { get; set; }
        public Guid CustomerId { get; set; }

        private Expense() { }
        internal Expense(
            Guid id,
            [NotNull] string number,
            [NotNull] DateTime expenseDate,
            [NotNull] Guid employeeId,
            [NotNull] Guid expenseTypeId,
            [NotNull] Guid customerId
            ) 
            : base(id)
        {
            SetNumber(number);
            ExpenseDate = expenseDate;
            EmployeeId = employeeId;
            ExpenseTypeId = expenseTypeId;
            CustomerId = customerId;
        }        
        internal Expense ChangeNumber([NotNull] string number)
        {
            SetNumber(number);
            return this;
        }
        private void SetNumber([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: ExpenseConsts.MaxNumberLength
                );
        }
    }
}
