using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.Bookings
{
    public class BookingUpdateDto
    {

        [Required]
        [StringLength(BookingConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
        public bool IsDone { get; set; }

        [Required]
        public Guid ResourceId { get; set; }
        public string Location { get; set; }
    }
}
