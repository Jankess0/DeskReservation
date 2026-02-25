using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;

namespace DeskReservation.IntegrationTests;

public class DeskControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _adminClient;
    private readonly HttpClient _userClient;
    private readonly HttpClient _anonymusClient;

    public DeskControllerTests(CustomWebApplicationFactory factory)
    {
        _adminClient = factory.CreateClient();
        _adminClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestAuth");
        
        _userClient = factory.CreateClient();
        _userClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestUser");
        
        _anonymusClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetAllDesks_WithoutJwtToken_ShouldReturnUnauthorized()
    {
        // ACT
        var response = await _anonymusClient.GetAsync("/api/Desk");
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAllDesks_WithAdminToken_ShouldReturnOk()
    {
        // ACT
        var response = await _adminClient.GetAsync("/api/Desk");
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}