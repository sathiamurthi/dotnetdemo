using Demo.Core.Domain.Models;
using MediatR;
using Demo.Core.Domain.Store;

namespace Demo.Core.Framework.Query
{
    public class GetProductsQuery : IRequest<List<Product>> {}

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<Product>>
    {
        private readonly FakeDataStore _fackStore;

        public GetProductsQueryHandler()
        {
            _fackStore = new FakeDataStore();
        }

        public Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = _fackStore.GetAllProducts().Result;

            return Task.FromResult(products);
        }
    }
}
