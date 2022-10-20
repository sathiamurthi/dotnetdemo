using Demo.Core.Domain.Models;
using MediatR;
using Demo.Core.Domain.Store;

namespace Demo.Core.Framework.Command
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public string Sku { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly FakeDataStore _dataStore;

        public DeleteProductCommandHandler()
        {
            _dataStore = new FakeDataStore();
        }

        public Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct =
                _dataStore.GetAllProducts().Result.FirstOrDefault(p => p.Sku.Equals(request.Sku));

            if (existingProduct != null)
            {
                var result = _dataStore.RemoveProduct(existingProduct);
                return Task.FromResult(result.Result);
            }

            throw new InvalidOperationException("Invalid product");
        }
    }
}
