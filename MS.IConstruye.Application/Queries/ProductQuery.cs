using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MS.IConstruye.Application
{
    public class ProductQuery : IProductQuery
    {
        private readonly string _connectionString = string.Empty;

        public ProductQuery(string connectionString) => _connectionString = connectionString;

        public async Task<ProductViewModel> Get(int Id)
        {
            ProductViewModel result;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@id", Id);

                result = await connection.QueryFirstAsync<ProductViewModel>(@"[dbo].[sp_get_product]", parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }
    }
}
