using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Employees
{
    public class Employee : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Guid DepartmentId { get; set; }
        public string EmployeeNumber { get; set; }
        public EmployeeGroup EmployeeGroup { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Bandwidth { get; set; }

        private Employee() { }
        internal Employee(
            Guid id,
            [NotNull] string name,
            [NotNull] string employeeNumber
            ) 
            : base(id)
        {
            SetName(name);
            SetEmployeeNumber(employeeNumber);
        }        
        internal Employee ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        internal Employee ChangeEmployeeNumber([NotNull] string employeeNumber)
        {
            SetEmployeeNumber(employeeNumber);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: EmployeeConsts.MaxNameLength
                );
        }
        private void SetEmployeeNumber([NotNull] string employeeNumber)
        {
            EmployeeNumber = Check.NotNullOrWhiteSpace(
                employeeNumber,
                nameof(employeeNumber),
                maxLength: EmployeeConsts.MaxNameLength
                );
        }
    }
}
