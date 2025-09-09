using Polly;
using Polly.Extensions.Http;

namespace backend.Policies
{
    public static class ResiliencePolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> Retry =>
            HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200));

        public static IAsyncPolicy<HttpResponseMessage> Timeout =>
            Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));

        public static IAsyncPolicy<HttpResponseMessage> CircuitBreaker =>
            HttpPolicyExtensions.HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}
