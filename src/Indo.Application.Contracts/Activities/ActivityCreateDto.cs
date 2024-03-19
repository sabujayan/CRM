using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.Activities
{
    public class ActivityCreateDto
    {

        [Required]
        [StringLength(ActivityConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
