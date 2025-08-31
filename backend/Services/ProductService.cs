using backend.Models;

namespace backend.Services
{
    public class ProductService
    {
        private readonly List<Product> _products = new();

        public IEnumerable<Product> GetAll() => _products;

        public Product? Get(string code) => _products.FirstOrDefault(p => p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        public bool Create(Product product)
        {
            if (_products.Any(p => p.Code.Equals(product.Code, StringComparison.OrdinalIgnoreCase)))
                return false;
            _products.Add(product);
            return true;
        }

        public bool Update(string code, Product product)
        {
            var existing = Get(code);
            if (existing == null) return false;

            if (!code.Equals(product.Code, StringComparison.OrdinalIgnoreCase) &&
                _products.Any(p => p.Code.Equals(product.Code, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            existing.Code = product.Code;
            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Quantity = product.Quantity;
            return true;
        }

        public bool Delete(string code)
        {
            var existing = Get(code);
            if (existing == null) return false;
            _products.Remove(existing);
            return true;
        }
    }
}
