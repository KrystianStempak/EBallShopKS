using System.Net;
using System.Net.Http.Json;
using EShop.Domain.ModelsDto;
using Xunit;
using FluentAssertions;
using Azure;
using NLog.Config;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class BallControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BallControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetBalls_ReturnsOk()
    {

        // Act
        var response = await _client.GetAsync("/api/ball");

        // Debugowanie odpowiedzi, jeśli wystąpi błąd 500
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("RESPONSE BODY:");
            Console.WriteLine(content);
        }

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }


}
