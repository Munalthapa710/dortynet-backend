using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using InternEntity = EmployeeApi.Model.Intern.Intern;

namespace EmployeeApi.Service.Intern
{
    public class InternService : IInternService
    {
        private readonly string _connString;

        public InternService(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string not found.");
        }

        public async Task<List<InternEntity>> GetAll()
        {
            using var connection = new SqlConnection(_connString);
            var interns = await connection.QueryAsync<InternEntity>(
                "[dbo].[GetAllInterns]",
                commandType: CommandType.StoredProcedure
            );
            return interns.ToList();
        }

        public async Task<InternEntity?> GetById(int id)
        {
            using var connection = new SqlConnection(_connString);
            return await connection.QueryFirstOrDefaultAsync<InternEntity>(
                "[dbo].[GetInternById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<InternEntity> Create(InternEntity intern)
        {
            using var connection = new SqlConnection(_connString);
            return await connection.QuerySingleAsync<InternEntity>(
                "[dbo].[CreateIntern]",
                new
                {
                    intern.Name,
                    intern.Description
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<InternEntity> Update(int id, InternEntity intern)
        {
            using var connection = new SqlConnection(_connString);
            var updatedIntern = await connection.QuerySingleOrDefaultAsync<InternEntity>(
                "[dbo].[UpdateIntern]",
                new
                {
                    Id = id,
                    intern.Name,
                    intern.Description
                },
                commandType: CommandType.StoredProcedure
            );

            return updatedIntern ?? throw new KeyNotFoundException($"Intern {id} was not found.");
        }

        public async Task<bool> Delete(int id)
        {
            using var connection = new SqlConnection(_connString);
            var deleted = await connection.ExecuteScalarAsync<int>(
                "[dbo].[DeleteIntern]",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return deleted > 0;
        }
    }
}
