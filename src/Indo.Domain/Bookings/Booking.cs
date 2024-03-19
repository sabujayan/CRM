using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Bookings
{
    public class Booking : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsDone { get; set; }
        public Guid ResourceId { get; set; }
        public string Location { get; set; }

        private Booking() { }
        internal Booking(
            Guid id,
            [NotNull] string name,
            [NotNull] DateTime startTime,
            [NotNull] DateTime endTime,
            [NotNull] Guid resourceId
            ) 
            : base(id)
        {
            SetName(name);
            StartTime = startTime;
            EndTime = endTime;
            ResourceId = resourceId;
        }        
        internal Booking ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: BookingConsts.MaxNameLength
                );
        }
    }
}
