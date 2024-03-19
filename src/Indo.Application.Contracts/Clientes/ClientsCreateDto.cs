using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Indo.Clientes
{
    public class ClientsCreateDto
    {
        [Required]
        [StringLength(ClientConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string projectnameist { get; set; }
    }
}
