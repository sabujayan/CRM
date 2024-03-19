using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Indo.ClientesAddress
{
    public class ClientsAddressCreateDto
    {
        [Required]
        [StringLength(ClientsAddressConsts.MaxNameLength)]
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public Guid ClientsId { get; set; }
    }
}
