using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Core.Domain.Models;
using Demo.Core.Domain.Data;
using System.Net.Http.Headers;

namespace Demo.Core.Domain.Store
{
    public class FakeDataStore
    {
        private readonly ProductsInMemory _productsInMemory;


        public FakeDataStore()
        {
            _productsInMemory = new ProductsInMemory();

        }

        public async Task AddProduct(Product product)
        {
            int index = _productsInMemory.Products.FindIndex(p => p.Sku.Equals(product.Sku));
            if (0 < index)
            {
                _productsInMemory.Products[index] = product;
            }
            else
            {
                _productsInMemory.Products.Add(product);
            }
            await Task.CompletedTask;
        }

        public async Task<bool> RemoveProduct(Product product) =>
            await Task.FromResult(_productsInMemory.Products.Remove(product));

        public async Task<List<Product>> GetAllProducts() => await Task.FromResult(_productsInMemory.Products);

        public async Task<Product> GetProductById(int id) =>
            await Task.FromResult(_productsInMemory.Products.Single(p => p.Id == id));

        public async Task EventOccured(Product product, string evt)
        {
            _productsInMemory.Products.Single(p => p.Id == product.Id).Name = $"{product.Name} evt: {evt}";
            await Task.CompletedTask;
        }
    }
}
