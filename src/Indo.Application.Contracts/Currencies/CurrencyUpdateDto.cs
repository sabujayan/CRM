using System.ComponentModel.DataAnnotations;


namespace Indo.Currencies
{
    public class CurrencyUpdateDto
    {

        [Required]
        [StringLength(CurrencyConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}
