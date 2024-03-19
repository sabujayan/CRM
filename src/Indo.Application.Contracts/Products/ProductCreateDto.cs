using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.Products
{
    public class ProductCreateDto
    {

        [Required]
        [StringLength(ProductConsts.MaxNameLength)]
        public string Name { get; set; }
        public float Price { get; set; }
        public float TaxRate { get; set; }
        public float RetailPrice { get; set; }
        public Guid UomId { get; set; }
    }
}
