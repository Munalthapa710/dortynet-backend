using ClientEntity = EmployeeApi.Model.Client.Client;
namespace EmployeeApi.Service.Client
{
    public interface IClientService
    {
        Task<List<ClientEntity>>GetAll();

        Task<ClientEntity?>GetById(int id);

        Task<ClientEntity>Create(ClientEntity client);

        Task<ClientEntity>Update(int id, ClientEntity client);

        Task<bool>Delete(int id);
    }
}
