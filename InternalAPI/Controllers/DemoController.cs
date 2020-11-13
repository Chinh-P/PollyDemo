using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _breakerPolicy;

        readonly AsyncRetryPolicy<HttpResponseMessage> _httpRetryPolicy;

        public DemoController(HttpClient httpClient, AsyncCircuitBreakerPolicy<HttpResponseMessage> breakerPolicy)
        {
            _httpClient = httpClient;
            _breakerPolicy = breakerPolicy;
            _httpRetryPolicy =
                Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            
            string requestEndpoint = $"NotStableAmazonInventory/{id}";

           
            HttpResponseMessage response = await _httpRetryPolicy.ExecuteAsync(
                () => _breakerPolicy.ExecuteAsync(
                () => _httpClient.GetAsync(requestEndpoint)
                ));

            if (response.IsSuccessStatusCode)
            {
                int itemsInStock = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                return Ok(itemsInStock);
            }

            return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync());
        }

        [HttpGet("pricing/{id}")]
        public async Task<IActionResult> GetPricing(int id)
        {
            string requestEndpoint = $"price/{id}";

            HttpResponseMessage response = await _httpRetryPolicy.ExecuteAsync(
                () => _breakerPolicy.ExecuteAsync(
                    () => _httpClient.GetAsync(requestEndpoint)));

            if (response.IsSuccessStatusCode)
            {
                decimal priceOfItem = JsonConvert.DeserializeObject<decimal>(await response.Content.ReadAsStringAsync());
                return Ok($"${priceOfItem}");
            }

            return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync());
        }

    }
}
