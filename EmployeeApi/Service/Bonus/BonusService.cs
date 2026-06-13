using Dapper;
using EmployeeApi.ViewModel.Bonus;
using EmployeeApi.ViewModel.Employee;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeApi.Service.Bonus
{
    public class BonusService : IBonusService
    {
        private readonly string _connString;

        public BonusService(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");
        }

        public async Task<IEnumerable<EmployeeDropdownViewModel>> GetEmployeeDropdown(string query)
        {
            using var connection = new SqlConnection(_connString);

            return await connection.QueryAsync<EmployeeDropdownViewModel>(
                "[dbo].[GetEmployeeDropdown]",
                new
                {
                    Query = query ?? string.Empty
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<EmployeeBonusResultViewModel?> CalculateEmployeeBonus(
            CalculateEmployeeBonusViewModel model)
        {
            using var connection = new SqlConnection(_connString);

            return await connection.QueryFirstOrDefaultAsync<EmployeeBonusResultViewModel>(
                "[dbo].[CalculateEmployeeBonus]",
                new
                {
                    model.EmployeeId,
                    model.Percentage
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
