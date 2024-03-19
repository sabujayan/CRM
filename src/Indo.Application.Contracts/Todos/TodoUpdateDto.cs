using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.Todos
{
    public class TodoUpdateDto
    {

        [Required]
        [StringLength(TodoConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
        public bool IsDone { get; set; }
        public string Location { get; set; }
    }
}
