using System.ComponentModel.DataAnnotations;

namespace Indo.CashAndBanks
{
    public class CashAndBankCreateDto
    {

        [Required]
        [StringLength(CashAndBankConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
