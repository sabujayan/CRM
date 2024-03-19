using Indo.Technologies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.Technologies
{
    public class IndexModel : IndoPageModel
    {
        [BindProperty]
        public PagedResultDto<TechnologyListViewModel> techList { get; set; }
        private readonly ITechnologyAppService _technologyappService;
        public IndexModel(ITechnologyAppService technologyappService)
        {
            _technologyappService = technologyappService;

        }
        public async Task OnGetAsync()
        {
            var input = new GetTechnologyInfoListDto
            {
                Filter = ""
            };
            var dto = await _technologyappService.GetTechnologyList(input);
            techList = ObjectMapper.Map<PagedResultDto<TechnologyReadDto>, PagedResultDto<TechnologyListViewModel>>(dto);
        }

        public class TechnologyListViewModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
          
        }
    }
}
