using Volo.Abp;

namespace Indo.LeadRatings
{
    public class LeadRatingAlreadyExistsException : BusinessException
    {
        public LeadRatingAlreadyExistsException(string name)
            : base("LeadRatingAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
