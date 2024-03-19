using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Indo.Projectes
{
    public class ProjectsCreateDto
    {
        [Required]
        [StringLength(ProjectsConsts.MaxNameLength)]
        public string Name { get; set; }
        public Guid ClientsId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float EstimateHours { get; set; }
        public string Notes { get; set; }
        public string Technology { get; set; }
        public string technologynameist { get; set; }
    }
}
