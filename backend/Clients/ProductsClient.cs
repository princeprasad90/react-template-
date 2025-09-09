namespace backend.Clients
{
    public interface IProductsClient
    {
        Task<string> GetProductsAsync(CancellationToken cancellationToken);
    }

    public class ProductsClient : IProductsClient
    {
        private readonly HttpClient _client;

        public ProductsClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetProductsAsync(CancellationToken cancellationToken)
        {
            return await _client.GetStringAsync("/products", cancellationToken);
        }
    }
}
