using Indo.Technologies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Indo.Projectes
{
    public class ProjectDto
    {
        [Required]
        [StringLength(ProjectsConsts.MaxNameLength)]
        public string Name { get; set; }
        public string ClientName { get; set; }
        public DateTime sStartDate { get; set; }
        public DateTime sEndDate { get; set; }
        public float EstimateHours { get; set; }
        public Guid ProjectsId { get; set; }
        public Guid TechnologyId { get; set; }
        public string tech { get; set; }
        public Guid id { get; set; }
        public string techdetails { get; set; }
    }
}
