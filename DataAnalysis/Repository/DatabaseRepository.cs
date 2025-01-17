using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.Repository
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly string _connectionString = "Server=localhost; Database=DataAnalysis; Integrated Security=True;";
        public async Task<List<Product>> SearchProductsAsync(string searchQuery)
        {
            var products = new List<Product>();
            string query = "SELECT TOP 50 product_title_fa, product_title_en FROM dbo.Product WHERE product_title_fa LIKE @searchQuery";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            products.Add(new Product
                            {
                                NameFa = reader["product_title_fa"] as string ?? "بدون نام",
                                NameEn = reader["product_title_en"] as string ?? "بدون نام لاتین"
                            });
                        }
                    }
                }
            }

            return products;
        }
    }
}

