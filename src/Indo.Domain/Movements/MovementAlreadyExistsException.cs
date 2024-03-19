using Volo.Abp;

namespace Indo.Movements
{
    public class MovementAlreadyExistsException : BusinessException
    {
        public MovementAlreadyExistsException(string number)
            : base("MovementAlreadyExists")
        {
            WithData("number", number);
        }
    }
}
