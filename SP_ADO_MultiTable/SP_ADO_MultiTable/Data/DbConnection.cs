using Microsoft.Data.SqlClient;

namespace SP_ADO_MultiTable.Data
{
    public class DbConnection
    {
        private readonly IConfiguration _config;

        public DbConnection(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_config.GetConnectionString("MyCon"));
        }
    }
}
