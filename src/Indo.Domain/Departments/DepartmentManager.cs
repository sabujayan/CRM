using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Departments
{
    public class DepartmentManager : DomainService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentManager(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<Department> CreateAsync(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _departmentRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new DepartmentAlreadyExistsException(name);
            }

            return new Department(
                GuidGenerator.Create(),
                name
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Department department,
           [NotNull] string newName)
        {
            Check.NotNull(department, nameof(department));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _departmentRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != department.Id)
            {
                throw new DepartmentAlreadyExistsException(newName);
            }

            department.ChangeName(newName);
        }
    }
}
