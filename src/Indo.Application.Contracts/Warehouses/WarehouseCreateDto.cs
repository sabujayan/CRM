using System.ComponentModel.DataAnnotations;

namespace Indo.Warehouses
{
    public class WarehouseCreateDto
    {

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
