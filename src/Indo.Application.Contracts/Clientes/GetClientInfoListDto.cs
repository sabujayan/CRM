using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Clientes
{
    public class GetClientInfoListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public string? nameFilter { get; set; }
        public string? emailFilter { get; set; }
        public string? phoneNoFilter { get; set; }
        public string ? addressFilter { get; set; }
        public string SortingColumn { get; set; }
        public string? clientProjectsFilter { get; set; }
    }
}
