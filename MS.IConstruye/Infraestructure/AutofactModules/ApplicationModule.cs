using Autofac;
using MS.IConstruye.Application;
using MS.IConstruye.Domain.Aggregates;
using MS.IConstruye.Repository;
using MS.IConstruye.Service;

namespace MS.IConstruye
{
    public class ApplicationModule:Autofac.Module
    {
        private readonly string _connectionString;
        public ApplicationModule(string connectionString) => _connectionString = connectionString;
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MemoryCacheService>().As<IMemoryCacheService>().InstancePerLifetimeScope();
            builder.Register(c => new ProductQuery(_connectionString)).As<IProductQuery>().InstancePerLifetimeScope();
            builder.Register(c => new OrderRepository(_connectionString)).As<IOrderRepository>().InstancePerLifetimeScope();
            builder.Register(c => new ProductRepository(_connectionString)).As<IProductRepository>().InstancePerLifetimeScope();
        }
    }
}
