 using EmployeeEntity = EmployeeApi.Model.Employee.Employee;

namespace EmployeeApi.Model.Intern
    
{
    public class Intern
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;


        public ICollection<EmployeeEntity> Employees { get; set; }
     = new List<EmployeeEntity>();

    }
}
