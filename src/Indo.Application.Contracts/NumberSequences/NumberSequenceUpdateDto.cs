using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.NumberSequences
{
    public class NumberSequenceUpdateDto
    {

        [Required]
        [StringLength(NumberSequenceConsts.MaxPrefixLength)]
        public string Suffix { get; set; }

        [Required]
        public NumberSequenceModules Module { get; set; }

        [Required]
        public Int64 NextNumber { get; set; }
    }
}
