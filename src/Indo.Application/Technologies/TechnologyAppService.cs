using Indo.Projectes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Indo.Technologies
{
    public class TechnologyAppService : IndoAppService, ITechnologyAppService
    {
        private readonly ITechnologyRepository _technologyRepository;
        private readonly TechnologyManager _technologyManager;

        public TechnologyAppService(
           ITechnologyRepository technologyRepository,
           TechnologyManager technologyManager

           )
        {
            _technologyRepository = technologyRepository;
            _technologyManager = technologyManager;
        }

        public async Task<TechnologyReadDto> GetAsync(Guid id)
        {
            var obj = await _technologyRepository.GetAsync(id);
            return ObjectMapper.Map<Technology, TechnologyReadDto>(obj);
        }

        public async Task<List<TechnologyReadDto>> GetListAsync()
        {
            var queryable = await _technologyRepository.GetQueryableAsync();
            var query = from technology in queryable
                        select new { technology };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Technology, TechnologyReadDto>(x.technology);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<PagedResultDto<TechnologyReadDto>> GetTechnologyList(GetTechnologyInfoListDto input)
        {
            try
            {
                var queryable = await _technologyRepository.GetQueryableAsync();

                var query = (from technology in queryable
                             select new
                             {
                                 Id = technology.Id,
                                 Name = technology.Name,
                                 Description = technology.Description,
                                 ParentId = technology.ParentId,
                                 technology
                             });

                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    string filter = input.Filter.ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(filter) || x.Description.Contains(filter));
                }

                if (!string.IsNullOrWhiteSpace(input.nameFilter) || !string.IsNullOrWhiteSpace(input.descriptionFilter))
                {
                    query = query.Where(y => (string.IsNullOrWhiteSpace(input.nameFilter) || y.Name.ToLower().Contains(input.nameFilter.ToLower())) &&
                    (string.IsNullOrWhiteSpace(input.descriptionFilter) || y.Description.Contains(input.descriptionFilter)));
                }

                var totalCount = await AsyncExecuter.CountAsync(query);

                if (!string.IsNullOrWhiteSpace(input.SortingColumn))
                {
                    string sortingColumn = input.SortingColumn;
                    bool isAscending = input.Sorting != "desc";

                    if (!string.IsNullOrEmpty(sortingColumn))
                    {
                        if (isAscending)
                        {
                            query = query.OrderBy(p => EF.Property<object>(p, sortingColumn));
                        }
                        else
                        {
                            query = query.OrderByDescending(p => EF.Property<object>(p, sortingColumn));
                        }
                    }
                }
                else
                {
                    query = query.OrderByDescending(x => x.Id);
                }

                var pagedQuery = query.Skip((input.SkipCount) * input.MaxResultCount).Take(input.MaxResultCount);

                var queryResult = await AsyncExecuter.ToListAsync(pagedQuery);

                var dtos = queryResult.Select(x =>
                {
                    var dto = ObjectMapper.Map<Technology, TechnologyReadDto>(x.technology);
                    dto.Id = x.Id;
                    dto.Name = x.Name;
                    if (x.Description == null)
                        dto.Description = "-";
                    else
                        dto.Description = x.Description;
                    dto.ParentId = x.ParentId == null ? new Guid() : (Guid)x.ParentId;
                    return dto;
                }).ToList();
                return new PagedResultDto<TechnologyReadDto>(
                    totalCount,
                    dtos
                );
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        public async Task<TechnologyReadDto> CreateAsync(TechnologyCreateDto input)
        {
            var obj = await _technologyManager.CreateAsync(
                input.Name
            );
            obj.Description = input.Description;
            if (input.ParentId == null || input.ParentId == Guid.Empty)
            {
                obj.ParentId = null;
            }
            else
            {
                obj.ParentId = input.ParentId;
            }
            await _technologyRepository.InsertAsync(obj);
            return ObjectMapper.Map<Technology, TechnologyReadDto>(obj);
        }

        public async Task UpdateAsync(Guid id, TechnologyUpdateDto input)
        {
            var obj = await _technologyRepository.FindAsync(id);

            if (obj == null)
            {
                throw new EntityNotFoundException(typeof(Technology), id);
            }
            if (obj.Name != input.Name)
            {
                await _technologyManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;
            if (input.ParentId == null || input.ParentId == Guid.Empty)
            {
                obj.ParentId = null;
            }
            else
            {
                obj.ParentId = input.ParentId;
            }

            await _technologyRepository.UpdateAsync(obj);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _technologyRepository.DeleteAsync(id);
        }
        public async Task<ListResultDto<TechnologyLookupDto>> GetTechnologyLookupAsync()
        {
            var list = await _technologyRepository.GetListAsync();
            return new ListResultDto<TechnologyLookupDto>(
                ObjectMapper.Map<List<Technology>, List<TechnologyLookupDto>>(list)
            );
        }

        public async Task<bool> DuplicateCheckforAdd(string name)
        {
            bool isExist = true;
            var dupcheck = _technologyRepository.Where(x => x.Name == name).FirstOrDefault();
            if (dupcheck != null)
                isExist = true;
            else
                isExist = false;
            return isExist;
        }

        public async Task<bool> DuplicateCheckforEdit(string name, Guid id)
        {
            bool isExist = true;
            var dupcheck = _technologyRepository.Where(x => x.Name == name).FirstOrDefault();
            if (dupcheck != null)
                if (dupcheck.Id == id)
                    isExist = false;
                else
                    isExist = true;
            else
                isExist = false;
            return isExist;

        }

    }
}
