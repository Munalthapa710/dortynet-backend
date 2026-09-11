//he part that receives HTTP requests from Postman, React, Angular, mobile apps
using Microsoft.AspNetCore.Identity;
using EmployeeApi.Service.Employee; // taking service to use it in the controller to perform operations on employee data
using EmployeeApi.Service.Notifications;
using EmployeeApi.ViewModel.Employee; // taking viewmodel to use it in the controller to receive data from the client when creating or updating an employee, and to validate the data before processing it in the service layer
using Microsoft.AspNetCore.Mvc; // Imports ASP.NET Core MVC features. ControllerBase ApiController Route HttpGet HttpPostOk() NotFound()
using EmployeeEntity = EmployeeApi.Model.Employee.Employee;
using Microsoft.AspNetCore.Authorization; //alias 

namespace EmployeeApi.Api.v1  {// Groups API controllers together.
    [ApiController] //This class is an API Controller. It enables features like automatic model validation and binding source inference.
    [Route("api/employee")] // base url for all endpoints in this controller, so all endpoints will start with api/employee
    public class EmployeeApiController : ControllerBase // ok NotFound badrequest Nocontent This class is a controller that handles HTTP requests related to employee operations. It inherits from ControllerBase, which provides basic functionality for handling HTTP requests and responses. The controller uses the IEmployeeService to perform operations on employee data and the CreateEmployeeViewModel and UpdateEmployeeViewModel to receive and validate data from the client when creating or updating an employee.
    {
        private readonly IEmployeeService _service; // declare a private readonly field to hold the instance of the IEmployeeService, which will be used to perform operations on employee data
        private readonly IEmployeeEmailSender _employeeEmailSender;

        public EmployeeApiController(IEmployeeService service, IEmployeeEmailSender employeeEmailSender) // constructor that takes an IEmployeeService parameter and initializes the _service field with it, allowing the controller to use the service to perform operations on employee data
        {
            _service = service; // initialize the _service field with the instance of the IEmployeeService provided through dependency injection, allowing the controller to use the service to perform operations on employee data
            _employeeEmailSender = employeeEmailSender;
        }

        [HttpGet] // This method will handle HTTP GET requests to the base URL (api/employee) and will return a list of all employees. It uses the _service to get all employees and returns them in the response with an HTTP 200 OK status code.
        [Authorize(Roles = "Manager,Employee")] // This attribute specifies that only users with the "Manager" role are authorized to access this endpoint.
        public async Task<IActionResult> Get([FromQuery] int page = 1,
      [FromQuery] int limit = 10,
      [FromQuery] string query = "",
            [FromQuery] string? departmentId = null)// This method will handle HTTP GET requests to the base URL (api/employee) and will return a list of all employees. It uses the _service to get all employees and returns them in the response with an HTTP 200 OK status code.
        {
            return Ok(await _service.GetPaged(page,limit,query,departmentId)); // Use the _service to get all employees and return them in the response with an HTTP 200 OK status code. The Ok() method creates an ObjectResult that produces a 200 OK response with the specified value (the list of employees) as the content.
        }

        [HttpGet("{id:int}")]// This method will handle HTTP GET requests to the URL api/employee/{id}, where {id} is an integer representing the employee's ID.
        [Authorize(Roles = "Manager,Employee")] // This attribute specifies that only users with the "Manager" or "Employee" roles are authorized to access this endpoint. It restricts access to the GetById method, ensuring that only users with the appropriate roles can retrieve employee details by ID.
        public async Task<IActionResult> GetById(int id) // This method will handle HTTP GET requests to the URL api/employee/{id}, where {id} is an integer representing the employee's ID. 
        {
            var employee = await _service.GetById(id); // Calls service.GetById(id) to retrieve the employee with the specified ID. The result is stored in the employee variable.
            return employee is null ? NotFound() : Ok(employee); // if else jastai ho 
        }

        [HttpPost("new")] // This method will handle HTTP POST requests to the base URL (api/employee/new) and will create a new employee using the data provided in the request body. It uses the CreateEmployeeViewModel to receive and validate the data from the client, and then it uses the _service to create a new employee. If the creation is successful, it returns the created employee in the response with an HTTP 201 Created status code.
        [Authorize(Roles ="Manager")]// This attribute specifies that only users with the "Manager" role are authorized to access this endpoint. It restricts access to the Create method, ensuring that only users with the appropriate role can create new employees.
        public async Task<IActionResult> Create([FromBody] CreateEmployeeViewModel model)  //Take data from request body.[FromBody] Convert to CreateEmployeeViewModel model and validate it. If the model is valid, create a new EmployeeEntity using the data from the model and use the _service to create a new employee. If the creation is successful, return the created employee in the response with an HTTP 201 Created status code.
        {
            var employee = new EmployeeEntity //create employee entity using the data from the model. The EmployeeEntity class represents the employee data that will be stored in the database. It has properties for Name, Email, Salary, and DepartmentId, which are populated with the corresponding values from the CreateEmployeeViewModel.
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Salary = model.Salary,
                ClientId = model.ClientId,
                DepartmentId = model.DepartmentId
            };

            var hasher = new PasswordHasher<EmployeeEntity>(); //create password hasher to hash the password before storing it in the database. The PasswordHasher class is a built-in class in ASP.NET Core that provides a way to hash passwords using a secure algorithm. It takes an instance of the EmployeeEntity class as a generic parameter, which allows it to access the properties of the employee entity when hashing the password.
            
            employee.PasswordHash = hasher.HashPassword(employee, model.Password); //hash the password using the hasher and store it in the employee entity. The HashPassword method takes two parameters: the employee entity and the plain text password from the model. It returns a hashed version of the password, which is then assigned to the Password property of the employee entity.
            employee.Role = model.Role; //assign the role from the model to the employee entity. The Role property of the employee entity is set to the value of the Role property from the CreateEmployeeViewModel, which allows the client to specify the role of the new employee when creating it.

            var createdEmployee = await _service.Create(employee);  //call service and Employee gets inserted into database.
            await _employeeEmailSender.SendAccountCreatedAsync(createdEmployee, model.Password);

            return CreatedAtAction(nameof(GetById), new { id = createdEmployee.Id }, createdEmployee); //201 Created
        }
        
        [HttpPut("{id:int}")] //PUT /api/employee/1 This method will handle HTTP PUT requests to the URL api/employee/{id}, where {id} is an integer representing the employee's ID. It will update the existing employee with the specified ID using the data provided in the request body. It uses the UpdateEmployeeViewModel to receive and validate the data from the client, and then it uses the _service to update the employee. If the employee with the specified ID does not exist, it returns an HTTP 404 Not Found status code. If the update is successful, it returns the updated employee in the response with an HTTP 200 OK status code.
        [Authorize(Roles = "Manager")] // This attribute specifies that only users with the "Manager" role are authorized to access this endpoint. It restricts access to the Update method, ensuring that only users with the appropriate role can update existing employees.)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeViewModel model)  // This method will handle HTTP PUT requests to the URL api/employee/{id}, where {id} is an integer representing the employee's ID. It will update the existing employee with the specified ID using the data provided in the request body. It uses the UpdateEmployeeViewModel to receive and validate the data from the client, and then it uses the _service to update the employee. If the employee with the specified ID does not exist, it returns an HTTP 404 Not Found status code. If the update is successful, it returns the updated employee in the response with an HTTP 200 OK status code.
        {
            var existingEmployee = await _service.GetById(id);
            if (existingEmployee is null)
            {
                return NotFound();
            }

            var employee = new EmployeeEntity //Create Updated Entity using the data from the model. The EmployeeEntity class represents the employee data that will be stored in the database. It has properties for Id, Name, Email, Salary, and DepartmentId, which are populated with the corresponding values from the UpdateEmployeeViewModel and the id parameter.
            {
                Id = id,
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = existingEmployee.PasswordHash,
                Role = existingEmployee.Role,
                Salary = model.Salary,
                ClientId = model.ClientId,
                DepartmentId = model.DepartmentId
            };

            return Ok(await _service.Update(employee)); // Calls service.
        }

        [HttpDelete("{id:int}")]  //DELETE /api/employee/1 
        [Authorize(Roles = "Manager")] 
        public async Task<IActionResult> Delete(int id) // receive 1
        {
            return await _service.Delete(id) ? NoContent() : NotFound(); // like if else if the employee with the specified ID was successfully deleted, it returns an HTTP 204 No Content status code. If the employee with the specified ID was not found, it returns an HTTP 404 Not Found status code.
        }
    }
}
