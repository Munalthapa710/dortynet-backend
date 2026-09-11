using EmployeeApi.ViewModel.Department;
using DepartmentEntity = EmployeeApi.Model.Department.Department;

namespace EmployeeApi.Service.Department
{
    public interface IDepartmentService
    {
        Task<List<DepartmentEntity>> GetAll();

        Task<DepartmentEntity?> GetById(int id);

        Task<DepartmentEntity> Create(DepartmentEntity department);

        Task<DepartmentEntity> Update(DepartmentEntity department);
        Task<IEnumerable<DepartmentDropdownViewModel>> GetDropdown(string query);

        Task<bool> Delete(int id);
    }
}