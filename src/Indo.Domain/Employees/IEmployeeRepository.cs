using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Indo.Employees
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
        Task GetEmployeeLookupAsync();
    }
}
