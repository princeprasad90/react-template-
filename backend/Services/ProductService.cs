using System.Collections.Generic;
using backend.Models;

namespace backend.Services
{
    public class ProductService
    {
        private readonly Dictionary<string, Product> _products = new(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<Product> GetAll() => _products.Values;

        public Product? Get(string code) =>
            _products.TryGetValue(code, out var product) ? product : null;

        public bool Create(Product product)
        {
            if (_products.ContainsKey(product.Code))
                return false;
            _products[product.Code] = product;
            return true;
        }

        public bool Update(string code, Product product)
        {
            if (!_products.ContainsKey(code)) return false;

            if (!code.Equals(product.Code, StringComparison.OrdinalIgnoreCase) &&
                _products.ContainsKey(product.Code))
            {
                return false;
            }

            _products.Remove(code);
            _products[product.Code] = product;
            return true;
        }

        public bool Delete(string code) => _products.Remove(code);
    }
}
