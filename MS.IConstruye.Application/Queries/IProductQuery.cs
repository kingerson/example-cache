using System.Threading.Tasks;

namespace MS.IConstruye.Application
{
    public interface IProductQuery
    {
        Task<ProductViewModel> Get(int Id);
    }
}
