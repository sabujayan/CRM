using Volo.Abp;

namespace Indo.Bookings
{
    public class BookingAlreadyExistsException : BusinessException
    {
        public BookingAlreadyExistsException(string name)
            : base("BookingAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
