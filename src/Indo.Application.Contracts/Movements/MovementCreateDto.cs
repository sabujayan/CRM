using Indo.NumberSequences;
using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.Movements
{
    public class MovementCreateDto
    {

        [Required]
        [StringLength(MovementConsts.MaxNumberLength)]
        public string Number { get; set; }

        [Required]
        public DateTime MovementDate { get; set; }

        [Required]
        public string SourceDocument { get; set; }

        [Required]
        public NumberSequenceModules Module { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public float Qty { get; set; }

        [Required]
        public Guid FromWarehouseId { get; set; }

        [Required]
        public Guid ToWarehouseId { get; set; }
    }
}
