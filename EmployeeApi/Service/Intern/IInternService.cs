using InternEntity = EmployeeApi.Model.Intern.Intern;

namespace EmployeeApi.Service.Intern
{
    public interface IInternService
    {
        Task<List<InternEntity>> GetAll();

        Task<InternEntity?> GetById(int id);

        Task<InternEntity> Create(InternEntity intern);

        Task<InternEntity> Update(int id, InternEntity intern);

        Task<bool> Delete(int id);
    }
}