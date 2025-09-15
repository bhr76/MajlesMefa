using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Entities
{
    public class DapperContext
    {
        private readonly IConfiguration _configration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configration)
        {
            _configration = configration;
            _connectionString = _configration.GetConnectionString("DapperConnectionString");
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
