using Demo.Core.Domain.Models;
using MediatR;
using Demo.Core.Domain.Store;

namespace Demo.Core.Framework.Command
{
    public class AddOrUpdateProductCommand : IRequest<bool>
    {
        public Product productModel { get; set; }
    }

    public class AddOrUpdateProductCommandHandler : IRequestHandler<AddOrUpdateProductCommand, bool>
    {
        private readonly FakeDataStore _dataStore;

        public AddOrUpdateProductCommandHandler()
        {
            _dataStore = new FakeDataStore();
        }
        
        public Task<bool> Handle(AddOrUpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct =
                _dataStore.GetAllProducts().Result.FirstOrDefault(p => p.Sku.Equals(request.productModel.Sku));

            if (existingProduct != null)
            {
               Task.Run(()=>
                   _dataStore.AddProduct(request.productModel)
                   );
                return Task.FromResult(true);
            }

            Task.Run(() =>
                   _dataStore.AddProduct(request.productModel)
                   );
            
            return Task.FromResult(true);
        }
    }
}
