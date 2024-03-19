using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Indo.ClientesContact
{
    public class ClientsContactUpdateDto
    {
        [Required]
        [StringLength(ClientsContactConsts.MaxNameLength)]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid ClientsId { get; set; }
    }
}
