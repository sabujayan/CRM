using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.Notes
{
    public class NoteCreateDto
    {

        [Required]
        public string Description { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
    }
}
