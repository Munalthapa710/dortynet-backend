//database ko model yaha 

namespace EmployeeApi.Model.Employee
{
    public class Employee
    {
        public int Id { get; set; } // get to read and set to write 

        public string Name { get; set; } = string.Empty; // string.name = string.Empty  

        public string Email { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public int DepartmentId { get; set; }

        public EmployeeApi.Model.Department.Department? Department { get; set; }  //inherit gareko 
    }
}