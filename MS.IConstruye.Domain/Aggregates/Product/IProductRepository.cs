using System.Threading.Tasks;

namespace MS.IConstruye.Domain.Aggregates
{
    public interface IProductRepository
    {
        Task<Product> Get(int Id);
    }
}
