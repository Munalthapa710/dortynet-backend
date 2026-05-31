using Dapper;
using EmployeeApi.ViewModel.Department;
using Microsoft.Data.SqlClient;
using System.Data;
using DepartmentEntity = EmployeeApi.Model.Department.Department;
namespace EmployeeApi.Service.Department
{
    public class DepartmentService : IDepartmentService
    {
        private readonly string _connstring;

        public DepartmentService(IConfiguration configuration)
        {
            _connstring = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<List<DepartmentEntity>> GetAll()
        {
            using var connection = new SqlConnection(_connstring);
            var department=await connection.QueryAsync<DepartmentEntity>(
                "[dbo].[GetAllDepartments]",
                 commandType: CommandType.StoredProcedure
                 ); 

            return department.ToList();
        }

        public async Task<DepartmentEntity?> GetById(int id)
        {
            using var connection = new SqlConnection(_connstring);
            var department = await connection.QueryFirstOrDefaultAsync<DepartmentEntity>(
                "[dbo].[GetDepartmentById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure
                );
         return department;
        }

        public async Task<DepartmentEntity> Create(DepartmentEntity department)
        {
           using var connection = new SqlConnection(_connstring);
            var createdepartment =await connection.QuerySingleAsync<DepartmentEntity>(
                "[dbo].[CreateDepartment]",
                new {
                    department.Name,
                    department.Description
                },
                commandType: CommandType.StoredProcedure
                );

            return createdepartment;
        }

        public async Task<DepartmentEntity> Update(DepartmentEntity department)
        {
            using var connection = new SqlConnection(_connstring);
            var updatedepartment = await connection.QuerySingleAsync<DepartmentEntity>(
                "[dbo].[UpdateDepartment]",
                new
                {
                    department.Id,
                    department.Name,
                    department.Description
                },
                commandType: CommandType.StoredProcedure
                );
            return updatedepartment?? throw new InvalidOperationException($"Department with ID {department.Id} not found.");
        }

        public async Task<bool> Delete(int id)
        {
            using var connection = new SqlConnection(_connstring);

            var affectedRows = await connection.ExecuteScalarAsync<int>(
                "[dbo].[DeleteDepartment]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public async Task<IEnumerable<DepartmentDropdownViewModel>> GetDropdown(string query)
        {
            using var connection = new SqlConnection(_connstring);

            return await connection.QueryAsync<DepartmentDropdownViewModel>(
                "[dbo].[GetDepartmentDropdown]",
                new
                {
                    Query = query ?? string.Empty
                },
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<List<DepartmentWithEmployeesViewModel>> GetDepartmentsWithEmployees()
        {
            using var connection = new SqlConnection(_connstring);

            var rows = await connection.QueryAsync<DepartmentEmployeeFlatViewModel>(
                "[dbo].[GetDepartmentsWithEmployees]",
                commandType: CommandType.StoredProcedure
            );

            var departments = rows
                .GroupBy(row => new
                {
                    row.DepartmentId,
                    row.DepartmentName,
                    row.Description
                })
                .Select(group => new DepartmentWithEmployeesViewModel
                {
                    DepartmentId = group.Key.DepartmentId,
                    DepartmentName = group.Key.DepartmentName,
                    Description = group.Key.Description,
                    Employees = group
                        .Where(row => row.EmployeeId.HasValue)
                        .Select(row => new DepartmentEmployeeViewModel
                        {
                            EmployeeId = row.EmployeeId!.Value,
                            EmployeeName = row.EmployeeName ?? string.Empty,
                            Email = row.Email ?? string.Empty,
                            Role = row.Role ?? string.Empty,
                            Status = row.Status ?? string.Empty
                        })
                        .ToList()
                })
                .ToList();

            return departments;
        }
    }
}