using System.ComponentModel.DataAnnotations;


namespace Indo.Resources
{
    public class ResourceUpdateDto
    {

        [Required]
        [StringLength(ResourceConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
