using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Indo.Clientes;
using Indo.ClientesAddress;
using Indo.ClientesContact;
using Indo.Customers;
using Indo.Departments;
using Indo.EmployeeClient;
using Indo.EmployeeSkillMatrices;
using Indo.EmployeeSkills;
using Indo.ProjectEmployee;
using Indo.Projectes;
using Indo.ProjectOrders;
using Indo.ProjectsTechnologies;
using Indo.PurchaseOrders;
using Indo.SalesOrders;
using Indo.ServiceOrders;
using Indo.Skills;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using static IdentityServer4.Models.IdentityResources;
using Volo.Abp.IdentityServer.Clients;
using Indo.ClientsProjects;

namespace Indo.Employees
{
    [Authorize]
    public class EmployeeAppService : IndoAppService, IEmployeeAppService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly EmployeeManager _employeeManager;
        private readonly IProjectOrderRepository _projectOrderRepository;
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IEmployeeSkillMatricesRepository _EmployeeSkillMatricesRepository;
        private readonly EmployeeSkillMatricesManager _EmployeeSkillMatricesManager;
        private readonly IClientsRepository _ClientsRepository;
        private readonly IEmployeesClientsMatricesRepository _EmployeesClientsMatricesRepository;
        private readonly EmployeesClientsMatricesManager _EmployeesClientsMatricesManager;

        private readonly IEmployeesProjectsMatricesRepository _EmployeesProjectsMatricesRepository;
        private readonly EmployeesProjectsMatricesManager _EmployeesProjectsMatricesManager;
        private readonly IProjectsRepository _ProjectsRepository;
        private readonly ProjectsManager _ProjectsManager;
        public EmployeeAppService(
            IEmployeeRepository employeeRepository,
            EmployeeManager employeeManager,
            IDepartmentRepository departmentRepository,
            IProjectOrderRepository projectOrderRepository,
            IServiceOrderRepository serviceOrderRepository,
            ISalesOrderRepository salesOrderRepository,
            IPurchaseOrderRepository purchaseOrderRepository,
            ISkillRepository skillRepository,
            IEmployeeSkillMatricesRepository EmployeeSkillMatricesRepository,
            EmployeeSkillMatricesManager EmployeeSkillMatricesManager,
            IClientsRepository ClientsRepository,
            IEmployeesClientsMatricesRepository EmployeesClientsMatricesRepository,
            EmployeesClientsMatricesManager EmployeesClientsMatricesManager,
            IProjectsRepository ProjectsRepository,
            ProjectsManager ProjectsManager, 
            IEmployeesProjectsMatricesRepository EmployeesProjectsMatricesRepository,
            EmployeesProjectsMatricesManager EmployeesProjectsMatricesManager
            )
        {
            _employeeRepository = employeeRepository;
            _employeeManager = employeeManager;
            _departmentRepository = departmentRepository;
            _projectOrderRepository = projectOrderRepository;
            _serviceOrderRepository = serviceOrderRepository;
            _salesOrderRepository = salesOrderRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _skillRepository = skillRepository;
            _EmployeeSkillMatricesRepository = EmployeeSkillMatricesRepository;
            _EmployeeSkillMatricesManager = EmployeeSkillMatricesManager;
            _ClientsRepository = ClientsRepository;
            _EmployeesClientsMatricesRepository = EmployeesClientsMatricesRepository;
            _EmployeesClientsMatricesManager = EmployeesClientsMatricesManager;
            _ProjectsRepository = ProjectsRepository;
            _ProjectsManager = ProjectsManager;
            _EmployeesProjectsMatricesRepository = EmployeesProjectsMatricesRepository;
            _EmployeesProjectsMatricesManager = EmployeesProjectsMatricesManager;
        }
        [Authorize("Employee_Get_By_ID")]
        public async Task<EmployeeReadDto> GetAsync(Guid id)
        {
            var queryableskillMatrices = await _EmployeeSkillMatricesRepository.GetQueryableAsync();
            var queryableSkills = await _skillRepository.GetQueryableAsync();
            var queryable = await _employeeRepository.GetQueryableAsync();
            var query = (from employee in queryable.Where(x => x.Id == id)
                         join department in _departmentRepository on employee.DepartmentId equals department.Id
                         join skillMatrices in queryableskillMatrices.Where(x => x.EmployeeId == id) on employee.Id equals skillMatrices.EmployeeId
                         select new { employee, department, skillMatrices });
            var totalCount = await query.CountAsync(); 
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Employee),id);
            }
            var dto = ObjectMapper.Map<Employee, EmployeeReadDto>(queryResult.employee);
            var empid = queryResult.employee.Id;
            var Emplist = new List<string>();
            var list = _EmployeesClientsMatricesRepository.Where(x => x.EmployeeId == empid).Select(x => x.ClientsId).ToList();
            foreach(var item in list)
            {
                Emplist.Add(item.ToString());
            }
            dto.EmployeeClientId = Emplist;
            var Projectlist = new List<string>();
            var lists = _EmployeesProjectsMatricesRepository.Where(x => x.EmployeeId == empid).Select(x => x.ProjectsId).ToList();
            foreach (var items in lists)
            {
                Projectlist.Add(items.ToString());
            }
            dto.EmployeeProjectId = Projectlist;
            var Skills = new List<string>();
            dto.DepartmentName = queryResult.department.Name;
            dto.SkillList = String.Join(',', Skills);
            return dto;
        }

        public async Task<PagedResultDto<EmployeeReadDto>> GetListAsync(GetEmployeeInfoListDto input)
        {
            var EmployeeList= GetEmployeeList(input);
            return EmployeeList.Result;
        }

        public async Task<List<EmployeeReadDto>> GetBuyerListAsync()
        {
            var queryable = await _employeeRepository.GetQueryableAsync();
            var query = from employee in queryable
                        join department in _departmentRepository on employee.DepartmentId equals department.Id
                        where employee.EmployeeGroup == EmployeeGroup.Buyer
                        select new { employee, department };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Employee, EmployeeReadDto>(x.employee);
                dto.DepartmentName = x.department.Name;
                return dto;
            }).ToList();

            return dtos;
        }
        public async Task<List<EmployeeReadDto>> GetSalesListAsync()
        {
            var queryable = await _employeeRepository.GetQueryableAsync();
            var query = from employee in queryable
                        join department in _departmentRepository on employee.DepartmentId equals department.Id
                        where employee.EmployeeGroup == EmployeeGroup.Sales
                        select new { employee, department };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Employee, EmployeeReadDto>(x.employee);
                dto.DepartmentName = x.department.Name;
                return dto;
            }).ToList();

            return dtos;
        }
        public async Task<ListResultDto<DepartmentLookupDto>> GetDepartmentLookupAsync()
        {
            var list = await _departmentRepository.GetListAsync();
            return new ListResultDto<DepartmentLookupDto>(
                ObjectMapper.Map<List<Department>, List<DepartmentLookupDto>>(list)
            );
        }
        [Authorize("Employee_Create_Authorize")]
        public async Task<EmployeeReadDto> CreateAsync(EmployeeCreateDto input)
        {
            try
            {
                var obj = await _employeeManager.CreateAsync(input.Name, input.EmployeeNumber);

                obj.Street = input.Street;
                obj.City = input.City;
                obj.State = input.State;
                obj.ZipCode = input.ZipCode;
                obj.Position = input.Position;
                obj.DepartmentId = input.DepartmentId;
                obj.EmployeeGroup = input.EmployeeGroup;
                obj.Phone = input.Phone;
                obj.Email = input.Email;
                obj.Bandwidth = input.Bandwidth;

                await _employeeRepository.InsertAsync(obj);

                if (!string.IsNullOrEmpty(input.SkillList))
                {
                    string[] skillsList = input.SkillList.Split(",");
                    int totalSkillElements = skillsList.Length;

                    for (int i = 0; i < totalSkillElements; i++)
                    {
                        EmployeeSkillMatricesCreateDto objEmpSkill = new EmployeeSkillMatricesCreateDto();
                        objEmpSkill.SkillsId = new Guid(skillsList[i]);
                        objEmpSkill.EmployeeId = obj.Id;
                        var empSkill = await _EmployeeSkillMatricesManager.CreateAsync(
                            objEmpSkill.EmployeeId,
                            objEmpSkill.SkillsId
                        );
                        await _EmployeeSkillMatricesRepository.InsertAsync(empSkill);
                    }
                }

                if (!string.IsNullOrEmpty(input.ClientNameList))
                {
                    string[] clientnameslist = input.ClientNameList.Split(",");
                    int totalClientsElements = clientnameslist.Length;

                    for (int i = 0; i < totalClientsElements; i++)
                    {
                        EmployeesClientsMatricesCreateDto objEmpClient = new EmployeesClientsMatricesCreateDto();
                        objEmpClient.ClientsId = new Guid(clientnameslist[i]);
                        objEmpClient.EmployeeId = obj.Id;
                        var empClient = await _EmployeesClientsMatricesManager.CreateAsync(
                            objEmpClient.EmployeeId,
                            objEmpClient.ClientsId
                        );
                        await _EmployeesClientsMatricesRepository.InsertAsync(empClient);
                    }
                }

                if (!string.IsNullOrEmpty(input.ProjectNameList))
                {
                    string[] projectsnamelist = input.ProjectNameList.Split(",");
                    int totalProjectsElements = projectsnamelist.Length;

                    for (int i = 0; i < totalProjectsElements; i++)
                    {
                        EmployeesProjectsMatricesCreateDto objEmpProject = new EmployeesProjectsMatricesCreateDto();
                        objEmpProject.ProjectsId = new Guid(projectsnamelist[i]);
                        objEmpProject.EmployeeId = obj.Id;
                        var empProject = await _EmployeesProjectsMatricesManager.CreateAsync(
                            objEmpProject.EmployeeId,
                            objEmpProject.ProjectsId
                        );
                        await _EmployeesProjectsMatricesRepository.InsertAsync(empProject);
                    }
                }

                return ObjectMapper.Map<Employee, EmployeeReadDto>(obj);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [Authorize("Employee_Update_Authorize")]
        public async Task UpdateAsync(Guid id, EmployeeUpdateDto input)
        {
            try
            {
                var existingEmployee = await _employeeRepository.GetAsync(id);

                if (existingEmployee.Name != input.Name)
                {
                    await _employeeManager.ChangeNameAsync(existingEmployee, input.Name);
                }

                if (existingEmployee.EmployeeNumber != input.EmployeeNumber)
                {
                    await _employeeManager.ChangeEmployeeNumberAsync(existingEmployee, input.EmployeeNumber);
                }

                existingEmployee.Street = input.Street;
                existingEmployee.City = input.City;
                existingEmployee.State = input.State;
                existingEmployee.ZipCode = input.ZipCode;
                existingEmployee.Position = input.Position;
                existingEmployee.DepartmentId = input.DepartmentId;
                existingEmployee.EmployeeGroup = input.EmployeeGroup;
                existingEmployee.Phone = input.Phone;
                existingEmployee.Email = input.Email;
                existingEmployee.Bandwidth = input.Bandwidth;

                await _employeeRepository.UpdateAsync(existingEmployee);

                if (input.skilllist != null)
                {
                    string[] projectList = input.skilllist.Split(",");
                    int totalproElements = projectList.Count();
                    var list = _EmployeeSkillMatricesRepository.Where(x => x.EmployeeId == id).Select(x => x.Id).ToList();
                    foreach (var item in list)
                    {
                        await _EmployeeSkillMatricesRepository.DeleteAsync(item);
                    }

                    for (int i = 0; i < totalproElements; i++)
                    {
                        EmployeeSkillMatricesCreateDto objclientPro = new EmployeeSkillMatricesCreateDto();
                        objclientPro.SkillsId = new Guid(projectList[i]);
                        objclientPro.EmployeeId = existingEmployee.Id;
                        var clientProobj = await _EmployeeSkillMatricesManager.CreateAsync(
                             objclientPro.EmployeeId,
                             objclientPro.SkillsId
                           );
                        await _EmployeeSkillMatricesRepository.InsertAsync(clientProobj);
                    }
                }
                else
                {
                    var list = _EmployeeSkillMatricesRepository.Where(x => x.EmployeeId == id && x.IsDeleted == false).Select(x => x.Id).ToList();
                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            await _EmployeeSkillMatricesRepository.DeleteAsync(item);
                        }
                    }

                }

                if (input.clientnamelist != null)
                {
                    string[] clientList = input.clientnamelist.Split(",");
                    int totalcleElements = clientList.Count();
                    var list = _EmployeesClientsMatricesRepository.Where(x => x.EmployeeId == id).Select(x => x.Id).ToList();
                    foreach (var item in list)
                    {
                        await _EmployeesClientsMatricesRepository.DeleteAsync(item);
                    }

                    for (int i = 0; i < totalcleElements; i++)
                    {
                        EmployeesClientsMatricesCreateDto objclientPro = new EmployeesClientsMatricesCreateDto();
                        objclientPro.ClientsId = new Guid(clientList[i]);
                        objclientPro.EmployeeId = existingEmployee.Id;
                        var clientProobj = await _EmployeesClientsMatricesManager.CreateAsync(
                             objclientPro.EmployeeId,
                             objclientPro.ClientsId
                           );
                        await _EmployeesClientsMatricesRepository.InsertAsync(clientProobj);
                    }
                }
                else
                {
                    var list = _EmployeesClientsMatricesRepository.Where(x => x.EmployeeId == id && x.IsDeleted == false).Select(x => x.Id).ToList();
                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            await _EmployeesClientsMatricesRepository.DeleteAsync(item);
                        }
                    }

                }

                if (input.projectnamelist != null)
                {
                    string[] projectList = input.projectnamelist.Split(",");
                    int totalproElements = projectList.Count();
                    var list = _EmployeesProjectsMatricesRepository.Where(x => x.EmployeeId == id).Select(x => x.Id).ToList();
                    foreach (var item in list)
                    {
                        await _EmployeesProjectsMatricesRepository.DeleteAsync(item);
                    }

                    for (int i = 0; i < totalproElements; i++)
                    {
                        EmployeesProjectsMatricesCreateDto objclientPro = new EmployeesProjectsMatricesCreateDto();
                        objclientPro.ProjectsId = new Guid(projectList[i]);
                        objclientPro.EmployeeId = existingEmployee.Id;
                        var clientProobj = await _EmployeesProjectsMatricesManager.CreateAsync(
                             objclientPro.EmployeeId,
                             objclientPro.ProjectsId
                           );
                        await _EmployeesProjectsMatricesRepository.InsertAsync(clientProobj);
                    }
                }
                else
                {
                    var list = _EmployeesProjectsMatricesRepository.Where(x => x.EmployeeId == id && x.IsDeleted == false).Select(x => x.Id).ToList();
                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            await _EmployeesProjectsMatricesRepository.DeleteAsync(item);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [Authorize("Employee_Delete_Authorize")]
        public async Task DeleteAsync(Guid id)
        {
            var cobj = await _employeeRepository.GetAsync(id);
            if (cobj != null)
            {
                await _employeeRepository.DeleteAsync(id);
            }
            var list = _EmployeeSkillMatricesRepository.Where(x => x.EmployeeId == id).Select(x => x.Id).ToList();
            foreach (var item in list)
            {
                await _EmployeeSkillMatricesRepository.DeleteAsync(item);
            }
            var list1 = _EmployeesClientsMatricesRepository.Where(x => x.EmployeeId == id).Select(x => x.Id).ToList();
            foreach (var item in list1)
            {
                await _EmployeesClientsMatricesRepository.DeleteAsync(item);
            }
            var list2 = _EmployeesProjectsMatricesRepository.Where(x => x.EmployeeId == id).Select(x => x.Id).ToList();
            foreach (var item in list2)
            {
                await _EmployeesProjectsMatricesRepository.DeleteAsync(item);
            }
        }
        public async Task<ListResultDto<SkillLookupDto>> GetSkillLookupAsync()
        {
            var list = await _skillRepository.GetListAsync();
            return new ListResultDto<SkillLookupDto>(
                ObjectMapper.Map<List<Skill>, List<SkillLookupDto>>(list)
            );
        }

        public async Task<ListResultDto<ClientsLookupDto>> GetClientLookupAsync()
        {
            var list = await _ClientsRepository.GetListAsync();
            return new ListResultDto<ClientsLookupDto>(
                ObjectMapper.Map<List<Clients>, List<ClientsLookupDto>>(list)
            );
        }

        public async Task<ListResultDto<ProjectsLookupDto>> GetProjectLookupAsync()
        {
            var list = await _ProjectsRepository.GetListAsync();
            return new ListResultDto<ProjectsLookupDto>(
                ObjectMapper.Map<List<Projects>, List<ProjectsLookupDto>>(list)
            );
        }

        public async Task<PagedResultDto<EmployeeReadDto>> GetEmployeeList(GetEmployeeInfoListDto input)
        {
            var queryableskillMatrices = await _EmployeeSkillMatricesRepository.GetQueryableAsync();
            var queryableSkills = await _skillRepository.GetQueryableAsync();
            var queryable = await _employeeRepository.GetQueryableAsync();
            var query = (from employee in queryable
                        join department in _departmentRepository on employee.DepartmentId equals department.Id
                        join skillMatrices in queryableskillMatrices on employee.Id equals skillMatrices.EmployeeId
                        select new 
                        {
                            Id = employee.Id,
                            Name = employee.Name,
                            Email = employee.Email,
                            Phone = employee.Phone,
                            Street = employee.Street,
                            Position = employee.Position,
                            State = employee.State,
                            City = employee.City,
                            ZipCode = employee.ZipCode,
                            DepartmentName = department.Name,
                            employee,department,skillMatrices
                        });

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                string filter = input.Filter.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(filter) ||
                                         x.Phone.Contains(filter) ||
                                         x.City.Contains(filter) ||
                                         x.Position.Contains(filter) 
                                         );
            }

            if (!string.IsNullOrWhiteSpace(input.nameFilter) || !string.IsNullOrWhiteSpace(input.phoneNoFilter) || !string.IsNullOrWhiteSpace(input.cityFilter) || !string.IsNullOrWhiteSpace(input.positionFilter) || !string.IsNullOrWhiteSpace(input.employeeDepartmentFilter))//!string.IsNullOrWhiteSpace(input.departmentFilter) || 
            {
                query = query.Where(y => (string.IsNullOrWhiteSpace(input.nameFilter) || y.Name.ToLower().Contains(input.nameFilter.ToLower())) &&
                                         (string.IsNullOrWhiteSpace(input.phoneNoFilter) || y.Phone.Contains(input.phoneNoFilter)) &&
                                         (string.IsNullOrWhiteSpace(input.cityFilter) || y.City.Contains(input.cityFilter.ToLower())) &&
                                         (string.IsNullOrWhiteSpace(input.positionFilter) || y.Position.Contains(input.positionFilter.ToLower())));                                         
            }

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
                query = query.OrderByDescending(x => x.employee.Id);
            }

            var pagedQuery = query.Skip((input.SkipCount) * input.MaxResultCount).Take(input.MaxResultCount);

            var queryResult = await AsyncExecuter.ToListAsync(pagedQuery);

            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Employee, EmployeeReadDto>(x.employee);
                dto.Name = x.Name;
                dto.Email = x.Email;
                dto.Phone = x.Phone;
                dto.Street = x.Street;
                dto.Position = x.Position;
                dto.State = x.State;
                dto.City = x.City;
                dto.ZipCode = x.ZipCode;
                dto.DepartmentName = x.DepartmentName;
                var clients = new List<string>();

                var empid = x.employee.Id;
                var clientList = _EmployeesClientsMatricesRepository.Where(x => x.EmployeeId == empid).ToList();
                string cname = string.Empty;
                foreach (var item in clientList)
                {
                    var name = _ClientsRepository.Where(x => x.Id == item.ClientsId).Select(x => x.Name).FirstOrDefault();
                    cname = cname + "," + name;
                }
                dto.ClientNameList = cname.TrimStart(',');

                var projectList = _EmployeesProjectsMatricesRepository.GetListAsync(x => x.EmployeeId == empid);
                string pname = string.Empty;
                if (projectList != null)
                {
                    foreach (var items in projectList.Result)
                    {
                        var projectname = _ProjectsRepository.Where(x => x.Id == items.ProjectsId).Select(x => x.Name).FirstOrDefault();
                        pname = pname + "," + projectname;
                    }
                }
                dto.ProjectNameList = pname.TrimStart(',');

                try
                {
                    var skillIdList = _EmployeeSkillMatricesRepository.GetListAsync(x => x.EmployeeId == empid);
                    var Skills = new List<string>();
                    if (skillIdList != null)
                    {
                        foreach (var item in skillIdList.Result)
                        {
                            Skills.Add(queryableSkills.Where(s => s.Id == item.SkillsId).FirstOrDefault().Name);

                        }
                    }
                    dto.SkillList = String.Join(',', Skills);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                return dto;
            });
            

            var nxtdtos=dtos.GroupBy(i => i.Id).Select(g => g.First()).ToList();

            var totalCount = nxtdtos.Count();

            return new PagedResultDto<EmployeeReadDto>(
                totalCount,
               nxtdtos
            );
        }


        public async Task<List<string>> GetClientProjectMapping(Guid id)
        {
            var result = new List<string>();
            var pid = id;
            var list = new List<string>();
            var lists = _EmployeesProjectsMatricesRepository.Where(x => x.EmployeeId == pid).Select(x => x.ProjectsId).ToList();
            foreach (var items in lists)
            {
                list.Add(items.ToString());
            }
            result = list;
            return result;
        }

        public async Task<List<string>> GetClientSkillMapping(Guid id)
        {
            var result = new List<string>();
            var pid = id;
            var list = new List<string>();
            var lists = _EmployeeSkillMatricesRepository.Where(x => x.EmployeeId == pid).Select(x => x.SkillsId).ToList();
            foreach (var items in lists)
            {
                list.Add(items.ToString());
            }
            result = list;
            return result;
        }

        public async Task<List<string>> GetClientMapping(Guid id)
        {
            var result = new List<string>();
            var pid = id;
            var list = new List<string>();
            var lists = _EmployeesClientsMatricesRepository.Where(x => x.EmployeeId == pid).Select(x => x.ClientsId).ToList();
            foreach (var items in lists)
            {
                list.Add(items.ToString());
            }
            result = list;
            return result;
        }

        public async Task<bool> CheckIfEmployeeNumberExist(string empNumber)
        {
            var isExist = false;
            if(empNumber!=null && empNumber != "")
            {
              var empList=  _employeeRepository.GetListAsync(x => x.EmployeeNumber == empNumber);
                if (empList.Result.Count > 0)
                {
                    isExist = true;
                }
            }
            return isExist;
        }

        public async Task<bool> DuplicateNameCheckforAdd(string name)
        {
            bool isVehicleExist = true;
            var dupcheck = _employeeRepository.Where(x => x.Name == name).FirstOrDefault();
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

        public async Task<bool> DuplicateNameCheckforEdit(string name, Guid id)
        {
            bool isVehicleExist = true;
            var dupcheck = _employeeRepository.Where(x => x.Name == name).FirstOrDefault();
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

        public async Task<bool> DuplicateIdCheckforAdd(string employeeNumber)
        {
            bool isVehicleExist = true;
            var dupcheck = _employeeRepository.Where(x => x.EmployeeNumber == employeeNumber).FirstOrDefault();
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

        public async Task<bool> DuplicateIdCheckforEdit(string employeeNumber, Guid id)
        {
            bool isVehicleExist = true;
            var dupcheck = _employeeRepository.Where(x => x.EmployeeNumber == employeeNumber).FirstOrDefault();
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
