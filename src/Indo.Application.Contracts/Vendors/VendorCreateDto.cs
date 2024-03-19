using System.ComponentModel.DataAnnotations;

namespace Indo.Vendors
{
    public class VendorCreateDto
    {

        [Required]
        [StringLength(VendorConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
