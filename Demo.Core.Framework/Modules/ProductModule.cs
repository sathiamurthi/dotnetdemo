using MediatR;
using Microsoft.Extensions.DependencyInjection;
namespace Demo.Core.Framework.Modules
{

        public static class ProductModule
        {
            public static IServiceCollection AddProductModule(this IServiceCollection serviceCollection)
            {
                serviceCollection.AddMediatR(typeof(ProductModule).Assembly);

                return serviceCollection;
            }
        }
    
}
