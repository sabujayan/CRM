using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Product
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public ProductUpdateViewModel Product { get; set; }
        public List<SelectListItem> Uoms { get; set; }

        private readonly IProductAppService _productAppService;
        public UpdateModel(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _productAppService.GetAsync(id);
            Product = ObjectMapper.Map<ProductReadDto, ProductUpdateViewModel>(dto);


            var uomLookup = await _productAppService.GetUomLookupAsync();
            Uoms = uomLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _productAppService.UpdateAsync(
                    Product.Id,
                    ObjectMapper.Map<ProductUpdateViewModel, ProductUpdateDto>(Product)
                );
                return NoContent();

            }
            catch (ProductAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class ProductUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [SelectItems(nameof(Uoms))]
            [DisplayName("UoM")]
            public Guid UomId { get; set; }

            [Required]
            [StringLength(ProductConsts.MaxNameLength)]
            public string Name { get; set; }

            [Required]
            public float Price { get; set; }

            [Required]
            [DisplayName("Tax Rate")]
            public float TaxRate { get; set; }

            [Required]
            [DisplayName("Retail Price")]
            public float RetailPrice { get; set; }
        }
    }
}
