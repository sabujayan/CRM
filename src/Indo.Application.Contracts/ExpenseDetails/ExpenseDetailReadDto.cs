using Indo.Expenses;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ExpenseDetails
{
    public class ExpenseDetailReadDto : AuditedEntityDto<Guid>
    {
        public Guid ExpenseId { get; set; }
        public string ExpenseNumber { get; set; }
        public string SummaryNote { get; set; }
        public float Price { get; set; }
        public string CurrencyName { get; set; }
        public string PriceString { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string EmployeeName { get; set; }
        public string ExpenseTypeName { get; set; }
    }
}
