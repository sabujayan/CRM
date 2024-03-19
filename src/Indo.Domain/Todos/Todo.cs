using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Todos
{
    public class Todo : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsDone { get; set; }
        public string Location { get; set; }

        private Todo() { }
        internal Todo(
            Guid id,
            [NotNull] string name,
            [NotNull] DateTime startTime,
            [NotNull] DateTime endTime
            ) 
            : base(id)
        {
            SetName(name);
            StartTime = startTime;
            EndTime = endTime;
        }        
        internal Todo ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: TodoConsts.MaxNameLength
                );
        }
    }
}
