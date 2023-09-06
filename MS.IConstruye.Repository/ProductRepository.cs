using Dapper;
using MS.IConstruye.Domain.Aggregates;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MS.IConstruye.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString = string.Empty;
        public ProductRepository(string connectionString) => _connectionString = connectionString;
        public async Task<Product> Get(int Id)
        {
            Product result;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@id", Id);

                result = await connection.QueryFirstAsync<Product>(@"[dbo].[sp_get_product]", parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }
    }
}
