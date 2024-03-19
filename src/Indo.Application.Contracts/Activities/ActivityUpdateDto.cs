using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.Activities
{
    public class ActivityUpdateDto
    {

        [Required]
        [StringLength(ActivityConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
