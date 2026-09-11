//database ko model yaha 
using System.Text.Json.Serialization;

namespace EmployeeApi.Model.Employee
{
    public class Employee
    {
        public int Id { get; set; } // get to read and set to write 

        public string Name { get; set; } = string.Empty; // string.name = string.Empty  

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty; 

        public decimal Salary { get; set; }

        public int DepartmentId { get; set; }

        public int? ClientId { get; set; }
        

        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Employee";
        public string Status { get; set; } = "active";
        public EmployeeApi.Model.Client.Client? Client { get; set; }


        public EmployeeApi.Model.Department.Department? Department { get; set; }  //inherit gareko 
    }
}
