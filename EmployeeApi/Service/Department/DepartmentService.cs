using EmployeeApi.Data;
using EmployeeApi.ViewModel.Department;
using Microsoft.EntityFrameworkCore;
using DepartmentEntity = EmployeeApi.Model.Department.Department;

namespace EmployeeApi.Service.Department
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DepartmentEntity>> GetAll()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<DepartmentEntity?> GetById(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<DepartmentEntity> Create(DepartmentEntity department)
        {
            _context.Departments.Add(department);

            await _context.SaveChangesAsync();

            return department;
        }

        public async Task<DepartmentEntity> Update(DepartmentEntity department)
        {
            var existingDepartment =
                await _context.Departments.FindAsync(department.Id)
                ?? throw new KeyNotFoundException(
                    $"Department {department.Id} was not found.");

            _context.Entry(existingDepartment)
                .CurrentValues
                .SetValues(department);

            await _context.SaveChangesAsync();

            return existingDepartment;
        }

        public async Task<bool> Delete(int id)
        {
            var department =
                await _context.Departments.FindAsync(id);

            if (department is null)
            {
                return false;
            }

            _context.Departments.Remove(department);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<DepartmentDropdownViewModel>> GetDropdown(string query)
        {
            query ??= string.Empty;

            return await _context.Departments
                .Where(d => query == "" || d.Name.Contains(query))
                .OrderBy(d => d.Name)
                .Select(d => new DepartmentDropdownViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .Take(20)
                .ToListAsync();
        }
    }
}