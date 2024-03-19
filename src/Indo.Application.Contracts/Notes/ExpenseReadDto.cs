using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Notes
{
    public class NoteReadDto : AuditedEntityDto<Guid>
    {
        public string Description { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
