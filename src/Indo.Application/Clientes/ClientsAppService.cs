using Indo.ClientesAddress;
using Indo.ClientesContact;
using Indo.ClientsProjects;
using Indo.Projectes;
using Indo.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;


namespace Indo.Clientes
{
    [Authorize]
    public class ClientsAppService : IndoAppService, IClientsAppService
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly IdentityUserManager _identityUserManager;
        private readonly ClientsManager _clientsManager;
        private readonly IClientsAddressRepository _clientsaddressRepository;
        private readonly ClientsAddressManager _clientsaddressmanager;
        private readonly IClientsContactRepository _clientsContactRepository;
        private readonly ClientsContactManager _ClientsContactManager;
        private readonly IProjectsRepository _ProjectsRepository;
        private readonly ClientsProjectsMatricesManager _clientsprojectsmatricesManager;
        private readonly IClientsProjectsMatricesRepository _clientsprojectsmatricesMatricesRepository;
        private readonly IAccountAppService _accountAppServiceRepository;



        public ClientsAppService(
        IClientsRepository clientsRepository,
        ClientsManager clientsManager,
        IClientsAddressRepository clientsaddressRepository,
        ClientsAddressManager clientsaddressmanager,
        IClientsContactRepository clientsContactRepository,
        ClientsContactManager clientsContactManager,
        IProjectsRepository ProjectsRepository,
        ClientsProjectsMatricesManager clientsprojectsmatricesManager,
        IClientsProjectsMatricesRepository clientsprojectsmatricesMatricesRepository,
        IdentityUserManager identityUserManager,
        AccountAppService accountAppService

        )
        {
            _clientsRepository = clientsRepository;
            _clientsManager = clientsManager;
            _clientsaddressRepository = clientsaddressRepository;
            _clientsaddressmanager = clientsaddressmanager;
            _clientsContactRepository = clientsContactRepository;
            _ClientsContactManager = clientsContactManager;
            _ProjectsRepository = ProjectsRepository;
            _clientsprojectsmatricesManager = clientsprojectsmatricesManager;
            _clientsprojectsmatricesMatricesRepository = clientsprojectsmatricesMatricesRepository;
            _identityUserManager = identityUserManager;
            _accountAppServiceRepository = accountAppService;

        }
        [Authorize("Clients_Get_By_Id")]
        public async Task<ClientsReadDto> GetAsync(Guid id)
        {
            var queryable = await _clientsRepository.GetQueryableAsync();
            var query = from client in queryable
                        join clientsaddress in _clientsaddressRepository on client.Id equals clientsaddress.ClientsId
                        join clientscontact in _clientsContactRepository on client.Id equals clientscontact.ClientsId
                        where client.Id == id
                        select new { client, clientsaddress, clientscontact };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Clients), id);
            }
            var dto = ObjectMapper.Map<Clients, ClientsReadDto>(queryResult.client);
            dto.Country = queryResult.clientsaddress.Country;
            dto.State = queryResult.clientsaddress.State;
            dto.City = queryResult.clientsaddress.City;
            dto.Address = queryResult.clientsaddress.Address;
            dto.Zip = queryResult.clientsaddress.Zip;
            dto.AddressId = queryResult.clientsaddress.Id;
            dto.ClientsId = queryResult.clientsaddress.ClientsId;
            dto.Email = queryResult.clientscontact.Email;
            dto.PhoneNumber = queryResult.clientscontact.PhoneNumber;
            dto.ContactId = queryResult.clientscontact.Id;

            var pid = queryResult.client.Id;
            var list = new List<string>();
            var lists = _clientsprojectsmatricesMatricesRepository.Where(x => x.ClientsId == pid).Select(x => x.ProjectsId).ToList();
            foreach (var items in lists)
            {
                list.Add(items.ToString());
            }
            dto.ClientProjectId = list;

            return dto;
        }

        public async Task<List<string>> GetClientProjectMapping(Guid id)
        {
            var result = new List<string>();
            var pid = id;
            var list = new List<string>();
            var lists = _clientsprojectsmatricesMatricesRepository.Where(x => x.ClientsId == pid).Select(x => x.ProjectsId).ToList();
            foreach (var items in lists)
            {
                list.Add(items.ToString());
            }
            result = list;
            return result;
        }

        public async Task<PagedResultDto<ClientsReadDto>> GetClientList(GetClientInfoListDto input)
        {
            try
            {
                var queryable = await _clientsRepository.GetQueryableAsync();

                var query = (from clients in queryable
                             join clientsaddress in _clientsaddressRepository on clients.Id equals clientsaddress.ClientsId
                             join clientscontact in _clientsContactRepository on clients.Id equals clientscontact.ClientsId
                             select new
                             {
                                 Id = clients.Id,
                                 Name = clients.Name,
                                 Email = clientscontact.Email,
                                 PhoneNumber = clientscontact.PhoneNumber,
                                 Address = clientsaddress.Address,
                                 Country = clientsaddress.Country,
                                 State = clientsaddress.State,
                                 City = clientsaddress.City,
                                 Zip = clientsaddress.Zip,
                                 clients
                             });

                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    string filter = input.Filter.ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(filter) ||
                                             x.Email.Contains(filter) ||
                                             x.PhoneNumber.Contains(filter) ||
                                             x.Address.Contains(filter)
                                             );
                }

                if (!string.IsNullOrWhiteSpace(input.nameFilter) || !string.IsNullOrWhiteSpace(input.emailFilter) || !string.IsNullOrWhiteSpace(input.phoneNoFilter) || !string.IsNullOrWhiteSpace(input.addressFilter) || !string.IsNullOrWhiteSpace(input.clientProjectsFilter))
                {
                    query = query.Where(y => (string.IsNullOrWhiteSpace(input.nameFilter) || y.Name.ToLower().Contains(input.nameFilter.ToLower())) &&
                                             (string.IsNullOrWhiteSpace(input.emailFilter) || y.Email.Contains(input.emailFilter)) &&
                                             (string.IsNullOrWhiteSpace(input.phoneNoFilter) || y.PhoneNumber.Contains(input.phoneNoFilter)) &&
                                             (string.IsNullOrWhiteSpace(input.addressFilter) || y.Address.Contains(input.addressFilter.ToLower())));

                    if (!string.IsNullOrWhiteSpace(input.clientProjectsFilter))
                    {
                        Guid pid = Guid.Parse(input.clientProjectsFilter);
                        query = query.Where(y => _clientsprojectsmatricesMatricesRepository.Any(p => p.ClientsId == y.clients.Id && p.ProjectsId == pid));
                    }
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

                    var dto = ObjectMapper.Map<Clients, ClientsReadDto>(x.clients);
                    dto.Email = x.Email;
                    dto.PhoneNumber = x.PhoneNumber;
                    int MaxLength = 15;
                    if (x.Address.Length > MaxLength)
                        dto.Address = x.Address.Substring(0, MaxLength) + "...";
                    else
                        dto.Address = x.Address;
                    dto.AddressDesc = x.Address;

                    dto.Country = x.Country;
                    dto.State = x.State;
                    dto.City = x.City;
                    dto.Zip = x.Zip;
                    var id = x.clients.Id;
                    var List = _clientsprojectsmatricesMatricesRepository.Where(x => x.ClientsId == id).ToList();
                    string pname = string.Empty;
                    foreach (var items in List)
                    {
                        var name = _ProjectsRepository.Where(x => x.Id == items.ProjectsId).Select(x => x.Name).FirstOrDefault();
                        pname = pname + "," + name;
                    }
                    dto.Projectnameist = pname.TrimStart(',');

                    if (dto.Projectnameist.Length > MaxLength)
                        dto.Projectnameist = pname.TrimStart(',').Substring(0, MaxLength) + "...";
                    else
                        dto.Projectnameist = pname.TrimStart(',');
                    dto.ProjectDesclist = pname.TrimStart(',');

                    return dto;
                }).ToList();

                return new PagedResultDto<ClientsReadDto>(
                    totalCount,
                    dtos
                );
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }


        public async Task<List<ClientsReadDto>> GetListAsync()
        {
            var queryable = await _clientsRepository.GetQueryableAsync();
            var query = from clients in queryable
                        join clientsaddress in _clientsaddressRepository on clients.Id equals clientsaddress.ClientsId
                        join clientscontact in _clientsContactRepository on clients.Id equals clientscontact.ClientsId
                        select new { clients, clientsaddress, clientscontact };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Clients, ClientsReadDto>(x.clients);
                dto.Country = x.clientsaddress.Country;
                dto.State = x.clientsaddress.State;
                dto.City = x.clientsaddress.City;
                dto.Zip = x.clientsaddress.Zip;
                dto.Email = x.clientscontact.Email;
                dto.PhoneNumber = x.clientscontact.PhoneNumber;
                return dto;
            }).ToList();
            return dtos;
        }

        [Authorize("Clients_Create_Authorize")]
        public async Task<ClientsReadDto> CreateAsync(ClientsCreateDto input)
        {
            var obj = await _clientsManager.CreateAsync(
                input.Name
            );
            await _clientsRepository.InsertAsync(obj);

            ClientsAddressCreateDto objcadd = new ClientsAddressCreateDto();
            objcadd.ClientsId = obj.Id;
            objcadd.Address = input.Address;
            objcadd.Country = input.Country;
            objcadd.State = input.State;
            objcadd.City = input.City;
            objcadd.Zip = input.Zip;

            var objcaddmng = await _clientsaddressmanager.CreateAsync(
              objcadd.Address,
              objcadd.ClientsId,
              objcadd.Country,
              objcadd.State,
              objcadd.City,
              objcadd.Zip
          );

            await _clientsaddressRepository.InsertAsync(objcaddmng);

            ClientsContactCreateDto conobj = new ClientsContactCreateDto();
            conobj.Email = input.Email;
            conobj.PhoneNumber = input.PhoneNumber;
            conobj.ClientsId = obj.Id;
            var objcon = await _ClientsContactManager.CreateAsync(
             conobj.Email,
             conobj.PhoneNumber,
             objcadd.ClientsId
            );
            await _clientsContactRepository.InsertAsync(objcon);


            if (input.projectnameist != null)
            {
                string[] projectList = input.projectnameist.Split(",");
                int totalproElements = projectList.Count();
                if (projectList != null)
                {
                    for (int i = 0; i < totalproElements; i++)
                    {
                        ClientsProjectsMatricesCreateDto objclientPro = new ClientsProjectsMatricesCreateDto();
                        objclientPro.ProjectsId = new Guid(projectList[i]);
                        objclientPro.ClientsId = obj.Id;
                        var clientProobj = await _clientsprojectsmatricesManager.CreateAsync(
                             objclientPro.ClientsId,
                             objclientPro.ProjectsId
                           );
                        await _clientsprojectsmatricesMatricesRepository.InsertAsync(clientProobj);
                    }
                }
            }


            return ObjectMapper.Map<Clients, ClientsReadDto>(obj);
        }

        [Authorize("Clients_Update_Authorize")]
        public async Task UpdateAsync(Guid id, ClientsUpdateDto input)
        {
            var cobj = await _clientsRepository.GetAsync(id);
            cobj.Name = input.Name;
            await _clientsRepository.UpdateAsync(cobj);

            var caddobj = await _clientsaddressRepository.GetAsync(input.AddressId);
            caddobj.Country = input.Country;
            caddobj.City = input.City;
            caddobj.State = input.State;
            caddobj.Zip = input.Zip;
            caddobj.Address = input.Address;
            await _clientsaddressRepository.UpdateAsync(caddobj);

            var conobj = await _clientsContactRepository.GetAsync(input.ContactId);
            conobj.Email = input.Email;
            conobj.PhoneNumber = input.PhoneNumber;
            await _clientsContactRepository.UpdateAsync(conobj);

            if (input.projectnameist != null)
            {
                string[] projectList = input.projectnameist.Split(",");
                int totalproElements = projectList.Count();
                var list = _clientsprojectsmatricesMatricesRepository.Where(x => x.ClientsId == id).Select(x => x.Id).ToList();
                foreach (var item in list)
                {
                    await _clientsprojectsmatricesMatricesRepository.DeleteAsync(item);
                }

                for (int i = 0; i < totalproElements; i++)
                {
                    ClientsProjectsMatricesCreateDto objclientPro = new ClientsProjectsMatricesCreateDto();
                    objclientPro.ProjectsId = new Guid(projectList[i]);
                    objclientPro.ClientsId = cobj.Id;
                    var clientProobj = await _clientsprojectsmatricesManager.CreateAsync(
                         objclientPro.ClientsId,
                         objclientPro.ProjectsId
                       );
                    await _clientsprojectsmatricesMatricesRepository.InsertAsync(clientProobj);
                }
            }
            else
            {
                var list = _clientsprojectsmatricesMatricesRepository.Where(x => x.ClientsId == id && x.IsDeleted == false).Select(x => x.Id).ToList();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        await _clientsprojectsmatricesMatricesRepository.DeleteAsync(item);
                    }
                }

            }

        }

        [Authorize("Clients_Delete_Authorize")]
        public async Task DeleteAsync(Guid id)
        {
            var cobj = await _clientsRepository.GetAsync(id);
            if (cobj != null)
            {
                await _clientsRepository.DeleteAsync(id);
            }
            var caddobj = _clientsaddressRepository.Where(x => x.ClientsId == id).FirstOrDefault();
            if (caddobj != null)
            {
                await _clientsaddressRepository.DeleteAsync(caddobj.Id);
            }

            var cContactobj = _clientsContactRepository.Where(x => x.ClientsId == id).FirstOrDefault();
            if (cContactobj != null)
            {
                await _clientsContactRepository.DeleteAsync(cContactobj.Id);
            }

            var list = _clientsprojectsmatricesMatricesRepository.Where(x => x.ClientsId == id).Select(x => x.Id).ToList();
            foreach (var item in list)
            {
                await _clientsprojectsmatricesMatricesRepository.DeleteAsync(item);
            }
        }

        public async Task<ListResultDto<ProjectsLookupDto>> GetProjectLookupAsync()
        {
            var list = await _ProjectsRepository.GetListAsync();
            return new ListResultDto<ProjectsLookupDto>(
                ObjectMapper.Map<List<Projects>, List<ProjectsLookupDto>>(list)
            );
        }

        public async Task<bool> DuplicateCheckforAdd(string name)
        {
            bool isVehicleExist = true;
            var dupcheck = _clientsRepository.Where(x => x.Name == name).FirstOrDefault();
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
            var dupcheck = _clientsRepository.Where(x => x.Name == name).FirstOrDefault();
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
        [AllowAnonymous]
        public async Task<ClientRegisterDto> ClientRegister(ClientRegisterDto input)
        {
            try
            {
                var obj = await _clientsManager.CreateAsync(
              input.Name
            );
                await _clientsRepository.InsertAsync(obj);
                ClientsAddressCreateDto objcadd = new ClientsAddressCreateDto();
                objcadd.ClientsId = obj.Id;
                objcadd.Address = input.Address;
                objcadd.Country = input.Country;
                objcadd.State = input.State;
                objcadd.City = input.City;
                objcadd.Zip = input.Zip;

                var objcaddmng = await _clientsaddressmanager.CreateAsync(
                  objcadd.Address,
                  objcadd.ClientsId,
                  objcadd.Country,
                  objcadd.State,
                  objcadd.City,
                  objcadd.Zip
                );
                await _clientsaddressRepository.InsertAsync(objcaddmng);
                ClientsContactCreateDto conobj = new ClientsContactCreateDto();
                conobj.Email = input.Email;
                conobj.PhoneNumber = input.PhoneNumber;
                conobj.ClientsId = obj.Id;

                var objcon = await _ClientsContactManager.CreateAsync(
                 conobj.Email,
                 conobj.PhoneNumber,
                 objcadd.ClientsId
                );
                await _clientsContactRepository.InsertAsync(objcon);

                var userDto = await _accountAppServiceRepository.RegisterAsync(
                     new RegisterDto
                     {
                         UserName = input.Name.Replace(" ","").ToLower(),
                         EmailAddress = input.Email,
                         Password = input.Password,
                         AppName = input.Name
                     }
                 );

                var cobj = await _clientsRepository.GetAsync(obj.Id);
                cobj.UserId = userDto.Id;
                await _clientsRepository.UpdateAsync(cobj);


                return ObjectMapper.Map<Clients, ClientRegisterDto>(obj);
            }
            catch(Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

    }
}
