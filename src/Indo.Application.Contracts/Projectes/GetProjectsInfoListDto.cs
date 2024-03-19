using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Projectes
{
    public class GetProjectsInfosListDto : PagedAndSortedResultRequestDto
    {
        public string? nameFilter { get; set; }
        public string? clientIdFilter { get; set; }
        public string? technologyFilter { get; set; }
    }
}
