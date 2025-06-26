using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Queries
{
    public class ApiBallInfoProvider : IBallInfoProvider
    {
        private readonly HttpClient _httpClient;

        public ApiBallInfoProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Ball?> GetBallByIdAsync(int ballId)
        {
            var response = await _httpClient.GetAsync($"/api/balls/{ballId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Ball>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
