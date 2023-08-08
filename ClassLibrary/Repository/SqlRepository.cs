using System.Data.SqlClient;
using TaskLibrary.Interfaces;

namespace TaskLibrary.Repository
{
    public class SqlRepository : IRepository
    {
        private readonly string _connectionString;

        public SqlRepository()
        {
            _connectionString = CreateConnectionString();
        }

        private string CreateConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "(localdb)\\MSSQLLocalDB";
            builder.InitialCatalog = "db";
            builder.IntegratedSecurity = true;

            string connectionString = builder.ConnectionString;

            return connectionString;
        }

        public IProductRepository ProductRepository  => new SqlProductRepository(_connectionString);
        public IOrderRepository OrderRepository => new SqlOrderRepository(_connectionString);
    }
}
