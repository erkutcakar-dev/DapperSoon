using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace DapperSoon.Context
{
    public class FootballStatsDb
    {
        private readonly IConfiguration _configuration;
        
        public FootballStatsDb(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection GetConnection()
        {
            var conn = new SqlConnection(_configuration.GetConnectionString("FootballDatabase"));
            conn.Open();
            return conn;
        }
    }
}
