using Dapper;
using MS.IConstruye.Domain.Aggregates;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MS.IConstruye.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;
        public OrderRepository(string connectionString) => _connectionString = connectionString;
        public async Task<int> Create(Order model)
        {
            var result = default(int);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@Name", model.Name);
                parameters.Add("@Email", model.Email);
                parameters.Add("@Address", model.Address);
                parameters.Add("@ProductId", model.ProductId);
                parameters.Add("@Quantity", model.Quantity);

                result = await connection.ExecuteScalarAsync<int>("[dbo].[sp_create_order]", parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }
    }
}
