using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Technologies
{
    public class GetTechnologyInfoListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public string? nameFilter { get; set; }
        public string? descriptionFilter { get; set; }
        public string SortingColumn { get; set; }
    }
}
