using System.Net.Http.Json;
using backend.Models;

namespace backend.Clients
{
    public interface IAuthClient
    {
        Task<AuthResponse?> ExchangeKeyAsync(string key, CancellationToken cancellationToken);
    }

    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _client;

        public AuthClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<AuthResponse?> ExchangeKeyAsync(string key, CancellationToken cancellationToken)
        {
            var response = await _client.GetAsync($"/exchange?key={Uri.EscapeDataString(key)}", cancellationToken);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: cancellationToken);
        }
    }
}
