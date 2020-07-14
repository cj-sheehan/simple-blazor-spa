using Dapper;
using Microsoft.Extensions.Configuration;
using NameApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NameApi.DataAccess.Repositories
{
    public interface INameRepository
    {
        Task<IEnumerable<NameModel>> GetAll();
        Task<NameModel> GetNameByIdAsync(int id);
        Task<NameModel> AddNameAsync(string name);
    }

    public class NameRepository : INameRepository
    {
        private readonly string _connectionString;

        public NameRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("NameDB");
        }

        public async Task<IEnumerable<NameModel>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<NameModel>(
                    "[dbo].[usp_NameGetAll]",
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<NameModel> GetNameByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<NameModel>(
                    "[dbo].[usp_NameGet]",
                    new { id },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<NameModel> AddNameAsync(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<NameModel>(
                    "[dbo].[usp_NameInsert]",
                    new { name },
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
