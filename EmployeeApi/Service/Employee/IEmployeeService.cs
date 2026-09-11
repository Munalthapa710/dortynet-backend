//what must be the using statement for this code file ?
using EmployeeApi.ViewModel.Employee;  // This line imports the Employee view model namespace, allowing access to classes and interfaces defined in that namespace. 
using EmployeeApi.ViewModel.Common;  // This line imports the Common view model namespace, allowing access to classes and interfaces defined in that namespace.
using EmployeeEntity = EmployeeApi.Model.Employee.Employee;  // This line creates an alias for the Employee class in the EmployeeApi.Model.Employee namespace, allowing it to be referenced as EmployeeEntity within this file.

namespace EmployeeApi.Service.Employee // This code defines the IEmployeeService interface, which declares methods for managing employee data in an asynchronous manner. The interface includes methods for retrieving all employees, retrieving an employee by ID, creating a new employee, updating an existing employee, and deleting an employee by ID. Each method returns a Task, indicating that the operations are asynchronous and will eventually return the specified result.
{
    public interface IEmployeeService
    {
        Task<List<EmployeeEntity>> GetAll(); //task mean asynchronous operation that will eventually return a List of EmployeeEntity objects.

        Task<EmployeeEntity?> GetById(int id); // This method retrieves an employee by their ID. It returns a Task that will eventually return an EmployeeEntity object if found, or null if not found.

        Task<EmployeeEntity> Create(EmployeeEntity employee); // EmployeeEntity employee is a parameter that represents the employee object to be created. The method will return the created EmployeeEntity object.

        Task<EmployeeEntity> Update(EmployeeEntity employee); // EmployeeEntity employee is a parameter that represents the employee object to be updated. The method will return the updated EmployeeEntity object.

        Task<bool> Delete(int id); // This method deletes an employee by their ID. It returns a Task that will eventually return a boolean value indicating whether the deletion was successful (true) or not (false).

        Task<EmployeeEntity?> GetByEmail(string email); // This method retrieves an employee by their email address. It returns a Task that will eventually return an EmployeeEntity object if found, or null if not found.
        Task<PagedResult<EmployeeListViewModel>> GetPaged(int page, int limit, string query, int? departmentId, string status); // This method retrieves a paginated list of employees based on the specified page number, limit (number of items per page), and an optional query string for filtering. It returns a Task that will eventually return a PagedResult containing a list of EmployeeListViewModel objects.
    

    }
}
