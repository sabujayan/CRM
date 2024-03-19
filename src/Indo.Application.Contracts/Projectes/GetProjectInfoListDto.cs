using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Projectes
{
    public class GetProjectInfoListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public string? nameFilter { get; set; }
        public string? startdateFilter { get; set; }
        public string? enddateFilter { get; set; }
        public string? estimateFilter { get; set; }
        public string? technologyFilter { get; set; }
        public string? clientFilter { get; set; }
        public string currentSort { get; set; }
        public string sortingColumn { get; set; }

    }
}
