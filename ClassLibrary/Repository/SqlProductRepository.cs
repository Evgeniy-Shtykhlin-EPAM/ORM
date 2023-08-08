using System.Data.SqlClient;
using Dapper;
using TaskLibrary.Entities;
using TaskLibrary.Interfaces;

namespace TaskLibrary.Repository
{
    public class SqlProductRepository : SqlBaseRepository, IProductRepository
    {
        public SqlProductRepository(string connectionString) : base(connectionString)
        {
        }

        public List<Product> GetProducts()
        {
            var sql = "select Id, Name, Description, Weight, Height, Width, Length from Product";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var products = connection.Query<Product>(sql).ToList();
                return products;
            }
        }

        public Product GetProductById(int id)
        {
            var sql = "select Id, Name, Description, Weight, Height, Width, Length from Product where Id=@Id";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var product = connection.Query<Product>(sql, new { Id = id }).FirstOrDefault();
                return product;
            }
        }

        public void UpdateProduct(Product product)
        {
            var sql = $"update Product " +
                      $"set Name = @name, " +
                      $"Description = @description, " +
                      $"Weight = @weight, " +
                      $"Height = @height, " +
                      $"Width=@width, " +
                      $"Length=@length" +
                          $" where Id = @Id";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                connection.Execute(sql, new
                {
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Weight,
                    product.Height,
                    product.Width,
                    product.Length
                });
            }
        }

        public void AddProduct(Product product)
        {
            var sql = $"Insert into Product(Name, Description, Weight, Height, Width, Length)" +
                      $" values(@name, @description, @weight, @height, @width, @length)";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                connection.Execute(sql, new
                {
                    product.Name,
                    product.Description,
                    product.Weight,
                    product.Height,
                    product.Width,
                    product.Length
                });
            }
        }

        public int DeleteProduct(int id)
        {
            var sql = "delete from Product where Id = @id";

            var sqlCheckCascade = "select Id, CreatedDate, UpdatedDate, ProductId, StatusId from [dbo].[Order] where ProductId = @Id";


            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var orders = connection.Query<Order>(sqlCheckCascade, new { Id = id }).ToList();

                if (!orders.Any())
                {
                    return connection.Execute(sql, new { Id = id });
                }

                Console.WriteLine($"Can't delete product with Id = {id}, exist orders with link to this product.");
                return 0;
            }
        }
    }
}
