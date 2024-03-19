using Volo.Abp;

namespace Indo.NumberSequences
{
    public class NumberSequenceAlreadyExistsException : BusinessException
    {
        public NumberSequenceAlreadyExistsException(string prefix)
            : base("NumberSequenceAlreadyExists")
        {
            WithData("prefix", prefix);
        }
    }
}
