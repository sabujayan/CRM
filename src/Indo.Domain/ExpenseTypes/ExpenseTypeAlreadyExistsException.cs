using Volo.Abp;

namespace Indo.ExpenseTypes
{
    public class ExpenseTypeAlreadyExistsException : BusinessException
    {
        public ExpenseTypeAlreadyExistsException(string name)
            : base("ExpenseTypeAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
