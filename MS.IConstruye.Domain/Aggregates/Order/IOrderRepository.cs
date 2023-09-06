using System.Threading.Tasks;

namespace MS.IConstruye.Domain.Aggregates
{
    public interface IOrderRepository
    {
        Task<int> Create(Order model);
    }
}
