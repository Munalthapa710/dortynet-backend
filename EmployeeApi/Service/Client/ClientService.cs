using EmployeeApi.Data;
using Microsoft.EntityFrameworkCore;
using ClientEntity = EmployeeApi.Model.Client.Client;
namespace EmployeeApi.Service.Client
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;
        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientEntity>> GetAll()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<ClientEntity?> GetById(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task<ClientEntity> Create(ClientEntity client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<ClientEntity> Update(int id, ClientEntity client)
        {
            var existingClient = await _context.Clients.FindAsync(id);
            if (existingClient == null)
                throw new InvalidOperationException("Client not found");

            _context.Entry(existingClient).CurrentValues.SetValues(client);
            await _context.SaveChangesAsync();
            return existingClient;
        }

        public async Task<bool> Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
