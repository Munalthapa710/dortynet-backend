using Dapper;
using EmployeeApi.Data;
using EmployeeApi.Model.Employee;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using InternEntity = EmployeeApi.Model.Intern.Intern;
namespace EmployeeApi.Service.Intern
{
    public class InternService : IInternService
    {

        private readonly string _connString;
        private readonly ApplicationDbContext _context;

        public InternService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connString = configuration.GetConnectionString("DefaultConnection") ?? 
                throw new InvalidOperationException("Connection string not found.");

        }

        public async Task<List<InternEntity>> GetAll()
        {
            //return await _context.Interns.ToListAsync();
            using var connection = new SqlConnection(_connString);
            var employees = await connection.QueryAsync<InternEntity>(
           "[dbo].[GetAllInterns]",
            commandType: CommandType.StoredProcedure
           );
            return employees.ToList();
        }

        public async Task<InternEntity?> GetById(int id)
        {
            //return await _context.Interns.FindAsync(id);
            using var connection = new SqlConnection(_connString);
            var employee = await connection.QueryFirstOrDefaultAsync<InternEntity>(
                "[dbo].[GetInternById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return employee;
        }

        public async Task<InternEntity> Create(InternEntity intern)
        {
            using var connection = new SqlConnection(_connString);
            var createdIntern = await connection.QuerySingleAsync<InternEntity>(
                "[dbo].[CreateIntern]",
                new
                {
                    intern.Name,
                    intern.Description
                },
                commandType: CommandType.StoredProcedure);

            return createdIntern;
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
                commandType: CommandType.StoredProcedure);
             return updatedIntern ?? throw new KeyNotFoundException($"Intern {intern.Id} was not found.");
        }

        public async Task<bool> Delete(int id)
        {
            using var connection = new SqlConnection(_connString);
            var deleted = await connection.ExecuteAsync(
                "[dbo].[DeleteIntern]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
            return deleted > 0;
               }

            
        }
    }
