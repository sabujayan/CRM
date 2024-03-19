﻿using Indo.Address;
using System;
using System.Collections.Generic;

namespace Indo.Leads
{
    public class LeadsDto
    {
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Industry { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string LastName { get; set; }
        public string Website { get; set; }
        public string SkypeId { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public ICollection<AddressDto> Addresses { get; set; }
    }
}