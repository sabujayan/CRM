using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.ProjectOrders
{
    public class ProjectOrderUpdateDto
    {

        [Required]
        [StringLength(ProjectOrderConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }

        [Required]
        public ProjectOrderRating Rating { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid SalesExecutiveId { get; set; }
    }
}
