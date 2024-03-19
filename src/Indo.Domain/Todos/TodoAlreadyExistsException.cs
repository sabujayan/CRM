using Volo.Abp;

namespace Indo.Todos
{
    public class TodoAlreadyExistsException : BusinessException
    {
        public TodoAlreadyExistsException(string name)
            : base("TodoAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
