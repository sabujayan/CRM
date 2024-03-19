using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.Customers
{
    public class CustomerUpdateDto
    {

        [Required]
        [StringLength(CustomerConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Guid LeadSourceId { get; set; }
        public Guid LeadRatingId { get; set; }
        public CustomerStage Stage { get; set; }
    }
}
