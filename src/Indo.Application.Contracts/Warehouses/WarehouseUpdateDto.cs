using System.ComponentModel.DataAnnotations;


namespace Indo.Warehouses
{
    public class WarehouseUpdateDto
    {

        [Required]
        [StringLength(WarehouseConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
