using Volo.Abp;

namespace Indo.Expenses
{
    public class ExpenseAlreadyExistsException : BusinessException
    {
        public ExpenseAlreadyExistsException(string number)
            : base("ExpenseAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
