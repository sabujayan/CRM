using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Indo.Technologies
{
    public class TechnologyUpdateDto
    {
        [Required]
        [StringLength(TechnologyConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ParentId { get; set; }
    }
}
