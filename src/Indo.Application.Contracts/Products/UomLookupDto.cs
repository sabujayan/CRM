using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Products
{
    public class UomLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
