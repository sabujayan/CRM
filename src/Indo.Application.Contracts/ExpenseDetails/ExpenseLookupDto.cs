using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ExpenseDetails
{
    public class ExpenseLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
