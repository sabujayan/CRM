using Indo.Clientes;
using Indo.ClientesContact;
using Indo.ProjectsTechnologies;
using Indo.Technologies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Indo.Projectes
{
    public class ProjectsAppService : IndoAppService, IProjectsAppService
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly ProjectsManager _projectsmanager;
        private readonly IClientsRepository _ClientsRepository;
        private readonly IClientsContactRepository _clientsContactRepository;
        private readonly ITechnologyRepository _TechnologyRepository;
        private readonly ProjectsTechnologyMatricesManager _ProjectsTechnologyMatricesManager;
        private readonly IProjectsTechnologyMatricesRepository _ProjectsTechnologyMatricesRepository;
        public ProjectsAppService(
            IProjectsRepository projectsRepository,
            ProjectsManager projectsmanager,
            IClientsRepository ClientsRepository,
            ITechnologyRepository TechnologyRepository,

               IClientsContactRepository clientsContactRepository,
            ProjectsTechnologyMatricesManager ProjectsTechnologyMatricesManager,
            IProjectsTechnologyMatricesRepository ProjectsTechnologyMatricesRepository
            )
        {
            _projectsRepository = projectsRepository;
            _projectsmanager = projectsmanager;
            _clientsContactRepository = clientsContactRepository;
            _ClientsRepository = ClientsRepository;
            _TechnologyRepository = TechnologyRepository;
            _ProjectsTechnologyMatricesManager = ProjectsTechnologyMatricesManager;
            _ProjectsTechnologyMatricesRepository = ProjectsTechnologyMatricesRepository;
        }
        public async Task<ProjectsReadDto> GetAsync(Guid id)
        {
            var queryable = await _projectsRepository.GetQueryableAsync();
            var query = from project in queryable
                        where project.Id == id
                        select new { project };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Projects), id);
            }
            var dto = ObjectMapper.Map<Projects, ProjectsReadDto>(queryResult.project);
            var pid = queryResult.project.Id;
            var Techlist = new List<string>();
            var lists = _ProjectsTechnologyMatricesRepository.Where(x => x.ProjectsId == pid).Select(x => x.TechnologyId).ToList();
            foreach (var items in lists)
            {
                Techlist.Add(items.ToString());
            }
            dto.TechnologyProjectId = Techlist;
            if (queryResult.project.StartDate == DateTime.MinValue)
                dto.sStartDate = String.Empty;
            else
                dto.sStartDate = queryResult.project.StartDate.ToString("dd/MM/yyyy");

            if (queryResult.project.EndDate == DateTime.MinValue)
                dto.sEndDate = String.Empty;
            else
                dto.sEndDate = queryResult.project.StartDate.ToString("dd/MM/yyyy");
            return dto;
        }
        public async Task<List<ProjectsReadDto>> GetListAsync()
        {
            var queryable = await _projectsRepository.GetQueryableAsync();
            var query = from project in queryable
                        join client in _ClientsRepository on project.ClientsId equals client.Id
                        select new { project, client };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Projects, ProjectsReadDto>(x.project);
                dto.ClientName = x.client.Name;
                dto.sStartDate = x.project.StartDate.ToString("dd/MM/yyyy");
                dto.sEndDate = x.project.EndDate.ToString("dd/MM/yyyy");
                var pid = x.project.Id;
                var techList = _ProjectsTechnologyMatricesRepository.Where(x => x.ProjectsId == pid).ToList();
                string techname = string.Empty;
                foreach (var items in techList)
                {
                    var tname = _TechnologyRepository.Where(x => x.Id == items.TechnologyId).Select(x => x.Name).FirstOrDefault();
                    techname = techname + "," + tname;
                }
                dto.technologynameist = techname.TrimStart(',');
               
                return dto;
            }).ToList();
            return dtos;

        }

        public async Task<PagedResultDto<ProjectsReadDto>> GetProjectList(GetProjectInfoListDto input)
        {
            var queryable = await _projectsRepository.GetQueryableAsync();
            var query = (from project in queryable
                         join client in _ClientsRepository on project.ClientsId equals client.Id
                         select new
                         {
                             Name = project.Name,
                             ClientName = client.Name,
                             sStartDate = project.StartDate,
                             sEndDate = project.EndDate,
                             EstimateHours = project.EstimateHours,
                             ClientsId = client.Id,
                             id = project.Id,
                             project

                         });
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(x => (string.IsNullOrWhiteSpace(input.Filter)
                || x.Name.ToLower().Contains(input.Filter.ToLower())
                || x.ClientName.ToLower().Contains(input.Filter.ToLower())));
            }

            if (!string.IsNullOrWhiteSpace(input.nameFilter) || !string.IsNullOrWhiteSpace(input.startdateFilter) || !string.IsNullOrWhiteSpace(input.enddateFilter) || !string.IsNullOrWhiteSpace(input.clientFilter) || !string.IsNullOrWhiteSpace(input.technologyFilter))
            {
                Guid clientid = Guid.Empty;
                if (input.clientFilter != null)
                {
                    clientid = new Guid(input.clientFilter);
                }

                query = query.Where(y => (string.IsNullOrWhiteSpace(input.nameFilter) || y.Name.ToLower().Contains(input.nameFilter.ToLower())) &&
                                         (string.IsNullOrWhiteSpace(input.startdateFilter) || y.sStartDate.Date.Equals(Convert.ToDateTime(input.startdateFilter).Date)) &&
                                         (string.IsNullOrWhiteSpace(input.enddateFilter) || y.sEndDate.Date.Equals(Convert.ToDateTime(input.enddateFilter).Date)) &&
                                         (string.IsNullOrWhiteSpace(input.clientFilter) || y.ClientsId.Equals(clientid)));

                if (!string.IsNullOrWhiteSpace(input.technologyFilter))
                {
                    Guid tid = Guid.Empty;
                    tid = new Guid(input.technologyFilter);
                    query = query.Where(y => _ProjectsTechnologyMatricesRepository.Any(p => p.ProjectsId == y.id && p.TechnologyId == tid));
                }

            }
            var totalCount = await AsyncExecuter.CountAsync(query);

            if (!string.IsNullOrWhiteSpace(input.sortingColumn))
            {
                string sortingColumn = input.sortingColumn;
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
                query = query.OrderByDescending(x => x.id);
            }
            var pagedQuery = query.Skip((input.SkipCount) * input.MaxResultCount).Take(input.MaxResultCount);

            var queryResult = await AsyncExecuter.ToListAsync(pagedQuery);

            var dtos = queryResult.Select(x =>
            {
                var dateFormat = "dd/MM/yyyy";
                var dto = ObjectMapper.Map<Projects, ProjectsReadDto>(x.project);
                dto.ClientName = x.ClientName;
                if (x.sStartDate == DateTime.MinValue)
                    dto.sStartDate = "-";
                else
                    dto.sStartDate = x.sStartDate.ToString(dateFormat);
               
                if (x.sEndDate == DateTime.MinValue)
                    dto.sEndDate = "-";
                else
                    dto.sEndDate = x.sEndDate.ToString(dateFormat);

                dto.EstimateHours = x.EstimateHours != null ? x.EstimateHours : 0;
                var pid = x.id;
                int MaxLength = 15;
                var techList = _ProjectsTechnologyMatricesRepository.Where(x => x.ProjectsId == pid).ToList();
                string techname = string.Empty;
                foreach (var items in techList)
                {
                    var tname = _TechnologyRepository.Where(x => x.Id == items.TechnologyId).Select(x => x.Name).FirstOrDefault();
                    techname = techname + "," + tname;
                }
                dto.technologynameist = techname.TrimStart(',');
                if (dto.technologynameist.Length > MaxLength)
                    dto.technologynameist = techname.TrimStart(',').Substring(0, MaxLength) + "...";
                else
                    dto.technologynameist = techname.TrimStart(',');
                dto.technologyDesc = techname.TrimStart(',');
                return dto;
            }).ToList();
            return new PagedResultDto<ProjectsReadDto>(
                totalCount,
                dtos
            );
        }


        public async Task<ProjectsReadDto> CreateAsync(ProjectsCreateDto input)
        {
            var obj = await _projectsmanager.CreateAsync(
                input.Name
            );
            obj.ClientsId = input.ClientsId;
            obj.StartDate = input.StartDate;
            obj.EndDate = input.EndDate;
            obj.EstimateHours = input.EstimateHours;
            obj.Notes = input.Notes;
            obj.Technology = input.Technology;
            await _projectsRepository.InsertAsync(obj);

            if (input.technologynameist != null)
            {
                string[] techList = input.technologynameist.Split(",");
                int totaltechElements = techList.Count();
                if (techList != null)
                {
                    for (int i = 0; i < totaltechElements; i++)
                    {
                        ProjectsTechnologiesCreateDto objProtech = new ProjectsTechnologiesCreateDto();
                        objProtech.TechnologyId = new Guid(techList[i]);
                        objProtech.ProjectsId = obj.Id;
                        var techProobj = await _ProjectsTechnologyMatricesManager.CreateAsync(
                             objProtech.ProjectsId,
                             objProtech.TechnologyId
                           );
                        await _ProjectsTechnologyMatricesRepository.InsertAsync(techProobj);
                    }

                }
            }

            return ObjectMapper.Map<Projects, ProjectsReadDto>(obj);

        }

        public async Task UpdateAsync(Guid id, ProjectsUpdateDto input)
        {
            var obj = await _projectsRepository.GetAsync(id);
            if (obj.Name != input.Name)
            {
                await _projectsmanager.ChangeNameAsync(obj, input.Name);
            }
            obj.ClientsId = input.ClientsId;
            obj.StartDate = input.StartDate;
            obj.EndDate = input.EndDate;
            obj.EstimateHours = input.EstimateHours;
            obj.Notes = input.Notes;
            obj.Technology = input.Technology;
            await _projectsRepository.UpdateAsync(obj);

            if (input.technologynameist != null)
            {
                string[] techList = input.technologynameist.Split(",");
                int totalproElements = techList.Count();
                var list = _ProjectsTechnologyMatricesRepository.Where(x => x.ProjectsId == id).Select(x => x.Id).ToList();
                foreach (var item in list)
                {
                    await _ProjectsTechnologyMatricesRepository.DeleteAsync(item);
                }

                for (int i = 0; i < totalproElements; i++)
                {
                    ProjectsTechnologiesCreateDto objProtech = new ProjectsTechnologiesCreateDto();
                    objProtech.TechnologyId = new Guid(techList[i]);
                    objProtech.ProjectsId = obj.Id;
                    var techProobj = await _ProjectsTechnologyMatricesManager.CreateAsync(
                         objProtech.ProjectsId,
                         objProtech.TechnologyId
                       );
                    await _ProjectsTechnologyMatricesRepository.InsertAsync(techProobj);
                }
            }
            else
            {
                var list = _ProjectsTechnologyMatricesRepository.Where(x => x.ProjectsId == id && x.IsDeleted == false).Select(x => x.Id).ToList();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        await _ProjectsTechnologyMatricesRepository.DeleteAsync(item);
                    }
                }

            }


        }
        public async Task DeleteAsync(Guid id)
        {
            if (_projectsRepository.Where(x => x.Id.Equals(id)).Any())
            {
                await _projectsRepository.DeleteAsync(id);
            }
            var list = _ProjectsTechnologyMatricesRepository.Where(x => x.ProjectsId == id).Select(x => x.Id).ToList();
            foreach (var item in list)
            {
                await _ProjectsTechnologyMatricesRepository.DeleteAsync(item);
            }
        }
        public async Task<List<string>> GetTechnologyProjectMapping(Guid id)
        {
            var result = new List<string>();
            var pid = id;
            var list = new List<string>();
            var lists = _ProjectsTechnologyMatricesRepository.Where(x => x.ProjectsId == pid).Select(x => x.TechnologyId).ToList();
            foreach (var items in lists)
            {
                list.Add(items.ToString());
            }
            result = list;
            return result;
        }

        public async Task<ListResultDto<ClientsLookupDto>> GetClientLookupAsync()
        {
            var list = await _ClientsRepository.GetListAsync();
            return new ListResultDto<ClientsLookupDto>(
                ObjectMapper.Map<List<Clients>, List<ClientsLookupDto>>(list)
            );
        }

        public async Task<ListResultDto<TechnologyLookupDto>> GetTechnologyLookupAsync()
        {
            var list = await _TechnologyRepository.GetListAsync();
            return new ListResultDto<TechnologyLookupDto>(
                ObjectMapper.Map<List<Technology>, List<TechnologyLookupDto>>(list)
            );
        }
        public async Task<bool> DuplicateCheckforClientRegister(string name)
        {
            bool isVehicleExist = true;
            var dupcheck = _clientsContactRepository.Where(x => x.Email == name).FirstOrDefault();
            if (dupcheck != null)
            {
                isVehicleExist = true;
            }
            else
            {
                isVehicleExist = false;
            }
            return isVehicleExist;
        }
        public async Task<bool> DuplicateCheckforClientRegisterPhoneNumber(string name)
        {
            bool isVehicleExist = true;
            var dupcheck = _clientsContactRepository.Where(x => x.PhoneNumber == name).FirstOrDefault();
            if (dupcheck != null)
            {
                isVehicleExist = true;
            }
            else
            {
                isVehicleExist = false;
            }
            return isVehicleExist;
        }

        public async Task<bool> DuplicateCheckforClient(string name)
        {
            bool isVehicleExist = true;
            var dupcheck = _ClientsRepository.Where(x => x.Name == name).FirstOrDefault();
            if (dupcheck != null)
            {
                isVehicleExist = true;
            }
            else
            {
                isVehicleExist = false;
            }
            return isVehicleExist;
        }
        public async Task<bool> DuplicateCheckforAdd(string name)
        {
            bool isVehicleExist = true;
            var dupcheck = _projectsRepository.Where(x => x.Name == name).FirstOrDefault();
            if (dupcheck != null)
            {
                isVehicleExist = true;
            }
            else
            {
                isVehicleExist = false;
            }
            return isVehicleExist;
        }

        public async Task<bool> DuplicateCheckforEdit(string name, Guid id)
        {
            bool isVehicleExist = true;
            var dupcheck = _projectsRepository.Where(x => x.Name == name).FirstOrDefault();
            if (dupcheck != null)
            {
                isVehicleExist = true;
            }
            else
            {
                isVehicleExist = false;
            }
            return isVehicleExist;
        }
    }
}
