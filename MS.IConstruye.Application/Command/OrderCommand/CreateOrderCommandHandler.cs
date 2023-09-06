using MS.IConstruye.Domain;
using MS.IConstruye.Domain.Aggregates;
using MS.IConstruye.Service;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MS.IConstruye.Application.Command.OrderCommand
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, int>
    {
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        public CreateOrderCommandHandler(
            IMemoryCacheService memoryCacheService,
            IProductRepository productRepository,
            IOrderRepository orderRepository
            )
        {
            _memoryCacheService = memoryCacheService ?? throw new ArgumentNullException(nameof(memoryCacheService));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            //Obtener el producto
            if (!_memoryCacheService.TryGetValue($"{ProductConstant.ProductMemory}_{request.ProductId}", out ProductViewModel product))
            {
                var productModel = await _productRepository.Get(request.ProductId);
                product = new ProductViewModel
                {
                    Id = productModel.Id,
                    Name = productModel.Name,
                    Description = productModel.Description,
                    Precio = productModel.Precio,
                    Stock = productModel.Stock
                };
            }

            if (product.Stock < request.Quantity)
                throw new IConstruyeBaseException(ProductConstant.StockNoDisponible);

            product.Stock = product.Stock - request.Quantity;

            // Modificar stock del producto - En Caché
            _memoryCacheService.Remove($"{ProductConstant.ProductMemory}_{request.ProductId}");
            _memoryCacheService.SetValue($"{ProductConstant.ProductMemory}_{request.ProductId}", product);

            // Crear la orden
            var order = new Order(request.Name, request.Email, request.Address,request.ProductId, request.Quantity,DateTime.Now);
            var result = await _orderRepository.Create(order);

            return result;
        }
    }
}
