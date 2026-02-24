using System.Net;
using FluentAssertions;

namespace DeskReservation.IntegrationTests;

public class DeskControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DeskControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetAllDesks_WithoutJwtToken_ShouldReturnUnauthorized()
    {
        // ACT
        var response = await _client.GetAsync("/api/Desk");
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}