using Microsoft.Data.SqlClient;

namespace ADO_CRUD.Data
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
