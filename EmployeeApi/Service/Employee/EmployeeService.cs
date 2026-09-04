//how to do

using EmployeeApi.Data;
using Microsoft.EntityFrameworkCore;
using EmployeeEntity = EmployeeApi.Model.Employee.Employee;

namespace EmployeeApi.Service.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context; //declare a private readonly field to hold the database context for accessing the employee table

        public EmployeeService(ApplicationDbContext context) //constructor that takes an ApplicationDbContext parameter and initializes the _context field with it (constructor dependency injection)
        {
            _context = context; //give to context the database context to access the database and perform operations on the employee table
        }

        public async Task<List<EmployeeEntity>> GetAll()
        {
            return await _context.Employees //access employee table from the database and include the related department data for each employee , _context.Employees return like select * from Employees, and Include(e => e.Department) is like a join with the Department table to get the department data for each employee
    .Include(e => e.Department) //include the related department data for each employee
    .ToListAsync(); //convert query result to a list and return it as a Task<List<EmployeeEntity>>
        }

        public async Task<EmployeeEntity?> GetById(int id) //find one employeeby id, if not found return null and if found return the employee with the related department data
        {
            return await _context.Employees //access employee table from the database
     .Include(e => e.Department) //include the related department data for the employee
     .FirstOrDefaultAsync(e => e.Id == id); //break it down e one employee in a row e.Id emp id comapre to method parameter ID ( find the employee with the specified id and return it as a Task<EmployeeEntity?>, if not found return null )
        }

        public async Task<EmployeeEntity> Create(EmployeeEntity employee) //create a new employee in the database and return the created employee with the related department data
        {
            _context.Employees.Add(employee); //add the new employee to the employee table in the database
            await _context.SaveChangesAsync(); //save the changes to the database and return the created employee with the related department data ( insert into like this )
            return employee; // return the created employee
        }

        public async Task<EmployeeEntity> Update(EmployeeEntity employee)  // update an existing employee in the database and return the updated employee
        {
            var existingEmployee = await _context.Employees.FindAsync(employee.Id) // find the existing employee in the database by id, if not found return null
                ?? throw new KeyNotFoundException($"Employee {employee.Id} was not found."); // if not found throw a KeyNotFoundException with a message indicating that the employee was not found

            _context.Entry(existingEmployee).CurrentValues.SetValues(employee); // update the existing employee with the new values from the employee parameter
            await _context.SaveChangesAsync(); // save the changes to the database and return the updated employee (update tablename like that in here )
            return existingEmployee; // return the updated employee 
        }

        public async Task<bool> Delete(int id) // delete an existing employee from the database by id and return true if the employee was deleted, false if the employee was not found
        {
            var employee = await _context.Employees.FindAsync(id); // find the existing employee in the database by id, if not found return null

            if (employee is null) // if the employee was not found, return false
            {
                return false;
            }

            _context.Employees.Remove(employee); // remove the existing employee from the employee table in the database
            await _context.SaveChangesAsync(); // save the changes to the database and return true if the employee was deleted
            return true; // return true if the employee was deleted
        }
    }
}
