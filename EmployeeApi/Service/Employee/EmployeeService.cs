//how to do
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using EmployeeApi.Data;
using Microsoft.EntityFrameworkCore;
using EmployeeEntity = EmployeeApi.Model.Employee.Employee;
using EmployeeApi.ViewModel.Common;
using EmployeeApi.ViewModel.Employee;

namespace EmployeeApi.Service.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context; //declare a private readonly field to hold the database context for accessing the employee table

        private readonly string _connString;
        public EmployeeService(ApplicationDbContext context, IConfiguration configuration) //constructor that takes an ApplicationDbContext parameter and initializes the _context field with it (constructor dependency injection)
        {
            _context = context; //give to context the database context to access the database and perform operations on the employee table this is for entity framework core 
            _connString = configuration.GetConnectionString("DefaultConnection") ?? //connection string is used to connect to the database, it is retrieved from the configuration file (appsettings.json) using the GetConnectionString method, if the connection string is not found, an InvalidOperationException is thrown with a message indicating that the connection string was not found
                throw new InvalidOperationException("Connection string not found."); // this is for dapper to connect to the database and perform operations on the employee table
        }

        public async Task<EmployeeEntity?> GetByEmail(string email) //find one employee by email, if not found return null and if found return the employee with the related department data
        {
            using var connection = new SqlConnection(_connString); //create a new SqlConnection object using the connection string from the configuration
            return await connection.QueryFirstOrDefaultAsync<EmployeeEntity>( //dapper method that executes a stored procedure to retrieve the employee with the specified email address, and returns the first result or null if no results are found
               "[dbo].[GetEmployeeByEmail]",
               new
               {
                   Email= email // initialize the Email parameter of the stored procedure with the email parameter of the method
               },
               commandType: CommandType.StoredProcedure); //commandType specifies that the command being executed is a stored procedure
        }

        //    public async Task<List<EmployeeEntity>> GetAll()
        //    {
        //        return await _context.Employees //access employee table from the database and include the related department data for each employee , _context.Employees return like select * from Employees, and Include(e => e.Department) is like a join with the Department table to get the department data for each employee
        //.Include(e => e.Department) //include the related department data for each employee
        //.ToListAsync(); //convert query result to a list and return it as a Task<List<EmployeeEntity>>
        //    }


        public async Task<List<EmployeeEntity>> GetAll() // retrieve all employees from the database and return them as a list of EmployeeEntity objects
        {
            using var connection = new SqlConnection(_connString); // create a new SqlConnection object using the connection string from the configuration

            var employees = await connection.QueryAsync<EmployeeEntity>( // dapper method that executes a stored procedure to retrieve all employees from the database and returns the result as an IEnumerable<EmployeeEntity>
                "[dbo].[GetAllEmployees]",
                commandType: CommandType.StoredProcedure); //execute the stored procedure to retrieve all employees from the database and return the result as a list of EmployeeEntity objects

            return employees.ToList(); //convert the result to a list and return it
        }

        //   public async Task<EmployeeEntity?> GetById(int id) //find one employeeby id, if not found return null and if found return the employee with the related department data
        //   {
        //       return await _context.Employees //access employee table from the database
        //.Include(e => e.Department) //include the related department data for the employee
        //.FirstOrDefaultAsync(e => e.Id == id); //break it down e one employee in a row e.Id emp id comapre to method parameter ID ( find the employee with the specified id and return it as a Task<EmployeeEntity?>, if not found return null )
        //   }

        public async Task<EmployeeEntity?> GetById(int id) // retrieve an employee by their ID from the database and return it as an EmployeeEntity object, or null if not found
        {
            using var connection = new SqlConnection(_connString); // create a new SqlConnection object using the connection string from the configuration

            return await connection.QueryFirstOrDefaultAsync<EmployeeEntity>( // dapper method that executes a stored procedure to retrieve the employee with the specified ID from the database and returns the first result or null if no results are found
                "[dbo].[GetEmployeeById]", // execute the stored procedure to retrieve the employee with the specified ID from the database
                new { Id = id }, // initialize the Id parameter of the stored procedure with the id parameter of the method
                commandType: CommandType.StoredProcedure); // specify that the command being executed is a stored procedure
        }


        //public async Task<EmployeeEntity> Create(EmployeeEntity employee) //create a new employee in the database and return the created employee with the related department data
        //{
        //    _context.Employees.Add(employee); //add the new employee to the employee table in the database
        //    await _context.SaveChangesAsync(); //save the changes to the database and return the created employee with the related department data ( insert into like this )
        //    return employee; // return the created employee
        //}
        public async Task<EmployeeEntity> Create(EmployeeEntity employee)
        {
            using var connection = new SqlConnection(_connString);

            var createdEmployee = await connection.QuerySingleAsync<EmployeeEntity>(
                "[dbo].[CreateEmployee]",
                new
                {
                    employee.Name,
                    employee.Email,
                    employee.PhoneNumber,
                    employee.PasswordHash,
                    employee.Role,
                    employee.Salary,
                    employee.DepartmentId,
                    employee.ClientId
                },
                commandType: CommandType.StoredProcedure);

            return createdEmployee;
        }


        //public async Task<EmployeeEntity> Update(EmployeeEntity employee)  // update an existing employee in the database and return the updated employee
        //{
        //    var existingEmployee = await _context.Employees.FindAsync(employee.Id) // find the existing employee in the database by id, if not found return null
        //        ?? throw new KeyNotFoundException($"Employee {employee.Id} was not found."); // if not found throw a KeyNotFoundException with a message indicating that the employee was not found

        //    _context.Entry(existingEmployee).CurrentValues.SetValues(employee); // update the existing employee with the new values from the employee parameter
        //    await _context.SaveChangesAsync(); // save the changes to the database and return the updated employee (update tablename like that in here )
        //    return existingEmployee; // return the updated employee 
        //}

        public async Task<EmployeeEntity> Update(EmployeeEntity employee)
        {
            using var connection = new SqlConnection(_connString);

            var updatedEmployee = await connection.QueryFirstOrDefaultAsync<EmployeeEntity>(
                "[dbo].[UpdateEmployee]",
                new
                {
                    employee.Id,
                    employee.Name,
                    employee.Email,
                    employee.PhoneNumber,
                    employee.PasswordHash,
                    employee.Role,
                    employee.Salary,
                    employee.DepartmentId,
                    employee.ClientId
                },
                commandType: CommandType.StoredProcedure);

            return updatedEmployee
                ?? throw new KeyNotFoundException($"Employee {employee.Id} was not found.");
        }

        //public async Task<bool> Delete(int id) // delete an existing employee from the database by id and return true if the employee was deleted, false if the employee was not found
        //{
        //    var employee = await _context.Employees.FindAsync(id); // find the existing employee in the database by id, if not found return null

        //    if (employee is null) // if the employee was not found, return false
        //    {
        //        return false;
        //    }

        //    _context.Employees.Remove(employee); // remove the existing employee from the employee table in the database
        //    await _context.SaveChangesAsync(); // save the changes to the database and return true if the employee was deleted
        //    return true; // return true if the employee was deleted
        //}
        public async Task<bool> Delete(int id)
        {
            using var connection = new SqlConnection(_connString);

            var affectedRows = await connection.ExecuteScalarAsync<int>(
                "[dbo].[DeleteEmployee]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);

            return affectedRows > 0;
        }

      public async Task<PagedResult<EmployeeListViewModel>> GetPaged(int page, int limit, string query ,string departmentId)
        {
            using var connection = new SqlConnection(_connString);

            var rows = await connection.QueryAsync<EmployeeListViewModel>(
                "[dbo].[GetEmployeesPaged]",
                new
                {
                    Page = page,
                    Limit = limit,
                    Query = query ?? string.Empty,
                    DepartmentId = departmentId ?? string.Empty
                },
                commandType: CommandType.StoredProcedure
            );

            var list = rows.ToList();

            return new PagedResult<EmployeeListViewModel>
            {
                Items = list,
                RowTotal = list.FirstOrDefault()?.RowTotal ?? 0,
                Page = page,
                Limit = limit
            };
        }
    }
}
