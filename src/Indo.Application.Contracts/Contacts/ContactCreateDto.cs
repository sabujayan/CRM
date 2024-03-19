using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.Contacts
{
    public class ContactCreateDto
    {

        [Required]
        [StringLength(ContactConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
    }
}
