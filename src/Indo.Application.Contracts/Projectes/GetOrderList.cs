using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Projectes
{
    public class GetOrderList : PagedAndSortedResultRequestDto
    {
        public string sortOrder { get; set; }
        public string currentSort { get; set; }
    }
}
