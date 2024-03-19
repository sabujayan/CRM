using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.Services
{
    public class ServiceUpdateDto
    {

        [Required]
        [StringLength(ServiceConsts.MaxNameLength)]
        public string Name { get; set; }
        public float Price { get; set; }
        public float TaxRate { get; set; }
        public Guid UomId { get; set; }
    }
}
