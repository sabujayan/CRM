using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.LeadSources
{
    public class LeadSourceUpdateDto
    {

        [Required]
        [StringLength(LeadSourceConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
