using Dapper;
using System.Data;
using System.Data.SqlClient;
using TaskLibrary.Entities;
using TaskLibrary.Interfaces;

namespace TaskLibrary.Repository
{
    public class SqlOrderRepository: SqlBaseRepository, IOrderRepository
    {
        private static IRepository Repository => new SqlRepository();

        public SqlOrderRepository(string connectionString) : base(connectionString)
        {
        }

        public List<Order> GetOrders()
        {
            var sql = "select Id, CreatedDate, UpdatedDate, ProductId, StatusId from [dbo].[Order]";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var orders = connection.Query<Order>(sql).ToList();
                return orders;
            }
        }
        
        public Order GetOrderById(int id)
        {
            var sql = $"select Id, CreatedDate, UpdatedDate, ProductId, StatusId from [dbo].[Order] where Id ={id}";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var order = connection.Query<Order>(sql, new { Id = id }).FirstOrDefault();
                return order;
            }
        }

        public void UpdateOrder(Order order)
        {
            var sql = $"update [dbo].[Order] set UpdatedDate = @updatedDate, StatusId = @statusId" +
                      $" where Id = @id";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                connection.Execute(sql, new
                {
                    order.Id,
                    order.StatusId,
                    updatedDate = DateTime.Now
                });
            }
        }

        public void AddOrder(Order order)
        {
            var sql = $"Insert into [dbo].[Order](CreatedDate, ProductId, StatusId)" +
                $" values(@createdDate, @productId, @statusId)";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                connection.Execute(sql, new
                {
                    order.CreatedDate,
                    order.ProductId,
                    order.StatusId
                });
            }
        }

        public void DeleteOrder(int id)
        {
            var sql = "delete from [dbo].[Order] where Id = @id";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                connection.Execute(sql, new { Id = id });
            }
        }

        public List<Order> GetOrdersFilteredByProductId(int productId)
        {
            var sql = "sp_FetchOrdersByProductId";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                return connection.Query<Order>(sql,new { productId }, commandType:CommandType.StoredProcedure).ToList();
            }
        }

        public int DeleteOrdersInBulkByProductId(int productId)
        {
            var sql = $"delete from [dbo].[Order] where ProductId = @productId";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                return connection.Execute(sql, new { ProductId = productId });
            }
        }
    }
}
