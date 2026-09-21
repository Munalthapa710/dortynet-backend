using Dapper;
using EmployeeApi.Logging;
using EmployeeApi.ViewModel.Bonus;
using EmployeeApi.ViewModel.Employee;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeApi.Service.Bonus
{
    public class BonusService : IBonusService
    {
        private readonly string _connString;
        private readonly IAppLogger _logger;

        public BonusService(IConfiguration configuration, IAppLogger logger)
        {
            _connString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");
            _logger = logger;
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
            try
            {
                _logger.Log(
                    LogType.Info,
                    () => "Calculating employee bonus.",
                    new { model.EmployeeId, model.Percentage });

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
            catch (Exception exception)
            {
                _logger.Log(
                    LogType.Error,
                    () => "Failed to calculate employee bonus.",
                    exception);

                throw;
            }
        }
    }
}
