using ShoppingCart.Domain.Interfaces;
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

        public EShop.Domain.Models.Ball? GetBallById(int ballId)
        {
            var response = _httpClient.GetAsync($"/api/balls/{ballId}").Result;

            if (!response.IsSuccessStatusCode)
                return null;

            var json = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<EShop.Domain.Models.Ball>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
