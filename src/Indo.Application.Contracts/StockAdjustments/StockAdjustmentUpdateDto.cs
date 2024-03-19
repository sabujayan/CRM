using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.StockAdjustments
{
    public class StockAdjustmentUpdateDto
    {

        [Required]
        [StringLength(StockAdjustmentConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }

        [Required]
        public StockAdjustmentType AdjustmentType { get; set; }

        [Required]
        public DateTime AdjustmentDate { get; set; }

        [Required]
        public Guid WarehouseId { get; set; }
    }
}
