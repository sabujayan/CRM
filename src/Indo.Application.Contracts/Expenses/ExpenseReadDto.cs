using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Expenses
{
    public class ExpenseReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeStreet { get; set; }
        public string EmployeeCity { get; set; }
        public string EmployeeState { get; set; }
        public string EmployeeZipCode { get; set; }
        public string EmployeePhone { get; set; }
        public string EmployeeEmail { get; set; }
        public Guid ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; }
        public string CurrencyName { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
