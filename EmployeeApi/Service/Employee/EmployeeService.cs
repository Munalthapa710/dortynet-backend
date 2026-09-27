using EmployeeApi.Data;
using EmployeeApi.ViewModel.Common;
using EmployeeApi.ViewModel.Employee;
using Microsoft.EntityFrameworkCore;
using EmployeeEntity = EmployeeApi.Model.Employee.Employee;

namespace EmployeeApi.Service.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeEntity?> GetByEmail(string email)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Client)
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<List<EmployeeEntity>> GetAll()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Client)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<EmployeeEntity?> GetById(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Client)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<EmployeeEntity> Create(EmployeeEntity employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<EmployeeEntity> Update(EmployeeEntity employee)
        {
            var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employee.Id)
                ?? throw new KeyNotFoundException($"Employee {employee.Id} was not found.");

            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.PhoneNumber = employee.PhoneNumber;
            existingEmployee.ProfileImagePath = employee.ProfileImagePath;
            existingEmployee.PasswordHash = employee.PasswordHash;
            existingEmployee.Role = employee.Role;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.DepartmentId = employee.DepartmentId;
            existingEmployee.ClientId = employee.ClientId;
            existingEmployee.Status = employee.Status;

            await _context.SaveChangesAsync();
            return existingEmployee;
        }

        public async Task<bool> Delete(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee is null)
            {
                return false;
            }

            employee.IsDeleted = true;
            employee.IsActive = false;
            employee.DeletedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<EmployeeListViewModel>> GetPaged(
            int page,
            int limit,
            string query,
            int? departmentId,
            string status)
        {
            page = page <= 0 ? 1 : page;
            limit = limit <= 0 ? 10 : limit;

            var employees = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Client)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var searchText = query.Trim();
                employees = employees.Where(e =>
                    e.Name.Contains(searchText) ||
                    e.Email.Contains(searchText) ||
                    e.PhoneNumber.Contains(searchText));
            }

            if (departmentId.HasValue)
            {
                employees = employees.Where(e => e.DepartmentId == departmentId.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                employees = employees.Where(e => e.Status == status);
            }

            var rowTotal = await employees.CountAsync();
            var items = await employees
                .OrderBy(e => e.Name)
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(e => new EmployeeListViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    ProfileImagePath = e.ProfileImagePath,
                    Salary = e.Salary,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department == null ? null : e.Department.Name,
                    ClientId = e.ClientId,
                    ClientName = e.Client == null ? null : e.Client.ClientName,
                    Role = e.Role,
                    Status = e.Status,
                    RowTotal = rowTotal
                })
                .ToListAsync();

            return new PagedResult<EmployeeListViewModel>
            {
                Items = items,
                RowTotal = rowTotal,
                Page = page,
                Limit = limit
            };
        }
    }
}