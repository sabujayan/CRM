using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Services
{
    public class UomLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
