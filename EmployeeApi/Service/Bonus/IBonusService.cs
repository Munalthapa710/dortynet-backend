using EmployeeApi.ViewModel.Bonus;
using EmployeeApi.ViewModel.Employee;

namespace EmployeeApi.Service.Bonus
{
    public interface IBonusService
    {
        Task<IEnumerable<EmployeeDropdownViewModel>> GetEmployeeDropdown(string query);

        Task<EmployeeBonusResultViewModel?> CalculateEmployeeBonus(
            CalculateEmployeeBonusViewModel model);
    }
}