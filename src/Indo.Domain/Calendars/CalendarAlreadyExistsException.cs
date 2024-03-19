using Volo.Abp;

namespace Indo.Calendars
{
    public class CalendarAlreadyExistsException : BusinessException
    {
        public CalendarAlreadyExistsException(string name)
            : base("CalendarAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
