using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.ProjectOrderDetails
{
    public class ProjectOrderDetailCreateDto
    {

        [Required]
        public Guid ProjectOrderId { get; set; }

        [Required]
        public string ProjectTask { get; set; }

        [Required]
        public float Quantity { get; set; }

        [Required]
        public float Price { get; set; }
    }
}
