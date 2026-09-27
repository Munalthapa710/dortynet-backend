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
            return await _context.Departments
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<DepartmentEntity?> GetById(int id)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<DepartmentEntity> Create(DepartmentEntity department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<DepartmentEntity> Update(DepartmentEntity department)
        {
            var existingDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.Id == department.Id)
                ?? throw new InvalidOperationException($"Department with ID {department.Id} not found.");

            existingDepartment.Name = department.Name;
            existingDepartment.Description = department.Description;
            await _context.SaveChangesAsync();
            return existingDepartment;
        }

        public async Task<bool> Delete(int id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (department is null)
            {
                return false;
            }

            department.IsDeleted = true;
            department.IsActive = false;
            department.DeletedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DepartmentDropdownViewModel>> GetDropdown(string query)
        {
            var departments = _context.Departments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                departments = departments.Where(d => d.Name.Contains(query));
            }

            return await departments
                .OrderBy(d => d.Name)
                .Select(d => new DepartmentDropdownViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();
        }

        public async Task<List<DepartmentWithEmployeesViewModel>> GetDepartmentsWithEmployees()
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .OrderBy(d => d.Name)
                .Select(d => new DepartmentWithEmployeesViewModel
                {
                    DepartmentId = d.Id,
                    DepartmentName = d.Name,
                    Description = d.Description,
                    Employees = d.Employees
                        .Where(e => !e.IsDeleted)
                        .OrderBy(e => e.Name)
                        .Select(e => new DepartmentEmployeeViewModel
                        {
                            EmployeeId = e.Id,
                            EmployeeName = e.Name,
                            Email = e.Email,
                            Role = e.Role,
                            Status = e.Status
                        })
                        .ToList()
                })
                .ToListAsync();
        }
    }
}