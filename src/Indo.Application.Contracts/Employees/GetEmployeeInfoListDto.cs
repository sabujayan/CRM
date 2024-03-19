using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Employees
{
    public class GetEmployeeInfoListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public string? nameFilter { get; set; }
        public string? phoneNoFilter { get; set; }
        public string? cityFilter { get; set; }
        public string? positionFilter { get; set; }
        public string SortingColumn { get; set; }
        public string? employeeDepartmentFilter { get; set; }
    }
}
