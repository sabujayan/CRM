using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Bookings
{
    public class BookingReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsDone { get; set; }
        public Guid ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string Location { get; set; }
    }
}
