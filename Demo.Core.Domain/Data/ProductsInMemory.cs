using Demo.Core.Domain.Models;

namespace Demo.Core.Domain.Data
{
    public class ProductsInMemory
    {
        private readonly List<Product> _productList;

        public ProductsInMemory()
        {
            _productList = new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    Name = "Milk",
                    Sku = "MILK",
                    Quantity = 10,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                },
                new Product()
                {
                    Id = 2,
                    Name = "Coffee",
                    Sku = "COFFEE",
                    Quantity = 10,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                },
                new Product()
                {
                    Id = 3,
                    Name = "Toast",
                    Sku = "TOAST",
                    Quantity = 10,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                },
                new Product()
                {
                    Id = 4,
                    Name = "Butter",
                    Sku = "BUTTER",
                    Quantity = 10,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                }
            };
        }

        public List<Product> Products => _productList;
    }
}
