using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Projectes
{
    public class ProjectsReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public Guid Id { get; set; }   
        public Guid ClientsId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float EstimateHours { get; set; }
        public string Notes { get; set; }
        public string Technology { get; set; }
        public string ClientName { get; set; }
        public string sStartDate { get; set; }
        public string sEndDate { get; set; }
        public string technologynameist { get; set; }
        public string technologyDesc { get; set; }
        public List<string> TechnologyProjectId { get; set; }
    }
}
