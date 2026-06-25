//Having separate ViewModels makes future changes easier. and can be used to add additional properties or validation rules specific to the update operation without affecting the create operation.

namespace EmployeeApi.ViewModel.Employee;

public class UpdateEmployeeViewModel : CreateEmployeeViewModel
{
}
