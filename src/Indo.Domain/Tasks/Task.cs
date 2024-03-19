using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Tasks
{
    public class Task : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsDone { get; set; }
        public string Location { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ActivityId { get; set; }

        private Task() { }
        internal Task(
            Guid id,
            [NotNull] string name,
            [NotNull] DateTime startTime,
            [NotNull] DateTime endTime,
            [NotNull] Guid customerId,
            [NotNull] Guid activityId
            ) 
            : base(id)
        {
            SetName(name);
            StartTime = startTime;
            EndTime = endTime;
            CustomerId = customerId;
            ActivityId = activityId;
        }        
        internal Task ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: TaskConsts.MaxNameLength
                );
        }
    }
}
