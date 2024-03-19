using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.Notes
{
    public class NoteUpdateDto
    {

        [Required]
        public string Description { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
    }
}
