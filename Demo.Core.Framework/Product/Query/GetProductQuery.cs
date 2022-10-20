using Demo.Core.Domain.Models;
using MediatR;
using Demo.Core.Domain.Store;

namespace Demo.Core.Framework.Query
{
    public class GetProductQuery : IRequest<Product>
    {
        public string Sku { get; set; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
    {
        private readonly FakeDataStore _fackStore;

        public GetProductQueryHandler()
        {
            _fackStore = new FakeDataStore();
        }

        public Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = _fackStore.GetAllProducts().Result.FirstOrDefault(p => p.Sku.Equals(request.Sku));
            if (product == null)
            {
                throw new InvalidOperationException("Invalid product");
            }

            return Task.FromResult(product);
        }
    }
}
