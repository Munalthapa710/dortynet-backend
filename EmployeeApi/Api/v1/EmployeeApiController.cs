//he part that receives HTTP requests from Postman, React, Angular, mobile apps

using EmployeeApi.Service.Employee; // taking service to use it in the controller to perform operations on employee data
using EmployeeApi.ViewModel.Employee; // taking viewmodel to use it in the controller to receive data from the client when creating or updating an employee, and to validate the data before processing it in the service layer
using Microsoft.AspNetCore.Mvc; // Imports ASP.NET Core MVC features. ControllerBase ApiController Route HttpGet HttpPostOk() NotFound()
using EmployeeEntity = EmployeeApi.Model.Employee.Employee; //alias 

namespace EmployeeApi.Api.v1  {// Groups API controllers together.
    [ApiController] //This class is an API Controller. It enables features like automatic model validation and binding source inference.
    [Route("api/employee")] // base url for all endpoints in this controller, so all endpoints will start with api/employee
    public class EmployeeApiController : ControllerBase // ok NotFound badrequest Nocontent This class is a controller that handles HTTP requests related to employee operations. It inherits from ControllerBase, which provides basic functionality for handling HTTP requests and responses. The controller uses the IEmployeeService to perform operations on employee data and the CreateEmployeeViewModel and UpdateEmployeeViewModel to receive and validate data from the client when creating or updating an employee.
    {
        private readonly IEmployeeService _service; // declare a private readonly field to hold the instance of the IEmployeeService, which will be used to perform operations on employee data

        public EmployeeApiController(IEmployeeService service) // constructor that takes an IEmployeeService parameter and initializes the _service field with it, allowing the controller to use the service to perform operations on employee data
        {
            _service = service; // initialize the _service field with the instance of the IEmployeeService provided through dependency injection, allowing the controller to use the service to perform operations on employee data
        }

        [HttpGet] // This method will handle HTTP GET requests to the base URL (api/employee) and will return a list of all employees. It uses the _service to get all employees and returns them in the response with an HTTP 200 OK status code.
        public async Task<IActionResult> Get() // This method will handle HTTP GET requests to the base URL (api/employee) and will return a list of all employees. It uses the _service to get all employees and returns them in the response with an HTTP 200 OK status code.
        {
            return Ok(await _service.GetAll()); // Use the _service to get all employees and return them in the response with an HTTP 200 OK status code. The Ok() method creates an ObjectResult that produces a 200 OK response with the specified value (the list of employees) as the content.
        }

        [HttpGet("{id:int}")] // This method will handle HTTP GET requests to the URL api/employee/{id}, where {id} is an integer representing the employee's ID.
        public async Task<IActionResult> GetById(int id) // This method will handle HTTP GET requests to the URL api/employee/{id}, where {id} is an integer representing the employee's ID. 
        {
            var employee = await _service.GetById(id); // Calls service.GetById(id) to retrieve the employee with the specified ID. The result is stored in the employee variable.
            return employee is null ? NotFound() : Ok(employee); // if else jastai ho 
        }

        [HttpPost] // This method will handle HTTP POST requests to the base URL (api/employee) and will create a new employee using the data provided in the request body. It uses the CreateEmployeeViewModel to receive and validate the data from the client, and then it uses the _service to create a new employee. If the creation is successful, it returns the created employee in the response with an HTTP 201 Created status code.
        public async Task<IActionResult> Create([FromBody] CreateEmployeeViewModel model)  //Take data from request body.[FromBody] Convert to CreateEmployeeViewModel model and validate it. If the model is valid, create a new EmployeeEntity using the data from the model and use the _service to create a new employee. If the creation is successful, return the created employee in the response with an HTTP 201 Created status code.
        {
            var employee = new EmployeeEntity //create employee entity using the data from the model. The EmployeeEntity class represents the employee data that will be stored in the database. It has properties for Name, Email, Salary, and DepartmentId, which are populated with the corresponding values from the CreateEmployeeViewModel.
            {
                Name = model.Name,
                Email = model.Email,
                Salary = model.Salary,
                 DepartmentId = model.DepartmentId
            };

            var createdEmployee = await _service.Create(employee);  //call service and Employee gets inserted into database.
            return CreatedAtAction(nameof(GetById), new { id = createdEmployee.Id }, createdEmployee); //201 Created
        }

        [HttpPut("{id:int}")] //PUT /api/employee/1 This method will handle HTTP PUT requests to the URL api/employee/{id}, where {id} is an integer representing the employee's ID. It will update the existing employee with the specified ID using the data provided in the request body. It uses the UpdateEmployeeViewModel to receive and validate the data from the client, and then it uses the _service to update the employee. If the employee with the specified ID does not exist, it returns an HTTP 404 Not Found status code. If the update is successful, it returns the updated employee in the response with an HTTP 200 OK status code.
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeViewModel model)  // This method will handle HTTP PUT requests to the URL api/employee/{id}, where {id} is an integer representing the employee's ID. It will update the existing employee with the specified ID using the data provided in the request body. It uses the UpdateEmployeeViewModel to receive and validate the data from the client, and then it uses the _service to update the employee. If the employee with the specified ID does not exist, it returns an HTTP 404 Not Found status code. If the update is successful, it returns the updated employee in the response with an HTTP 200 OK status code.
        {
            if (await _service.GetById(id) is null)
            {
                return NotFound();
            }

            var employee = new EmployeeEntity //Create Updated Entity using the data from the model. The EmployeeEntity class represents the employee data that will be stored in the database. It has properties for Id, Name, Email, Salary, and DepartmentId, which are populated with the corresponding values from the UpdateEmployeeViewModel and the id parameter.
            {
                Id = id,
                Name = model.Name,
                Email = model.Email,
                Salary = model.Salary,
                DepartmentId = model.DepartmentId
            };

            return Ok(await _service.Update(employee)); // Calls service.
        }

        [HttpDelete("{id:int}")]  //DELETE /api/employee/1 
        public async Task<IActionResult> Delete(int id) // receive 1
        {
            return await _service.Delete(id) ? NoContent() : NotFound(); // like if else if the employee with the specified ID was successfully deleted, it returns an HTTP 204 No Content status code. If the employee with the specified ID was not found, it returns an HTTP 404 Not Found status code.
        }
    }
}
