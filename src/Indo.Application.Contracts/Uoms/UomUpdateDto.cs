using System.ComponentModel.DataAnnotations;


namespace Indo.Uoms
{
    public class UomUpdateDto
    {

        [Required]
        [StringLength(UomConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
