using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Departments
{
    public interface IDepartmentRepository : IRepository<Department, Guid>
    {
    }
}
