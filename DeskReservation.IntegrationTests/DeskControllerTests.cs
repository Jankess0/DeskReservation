using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DeskReservation.DTOs;
using FluentAssertions;
using Xunit.Abstractions;

namespace DeskReservation.IntegrationTests;

public class DeskControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly HttpClient _adminClient;
    private readonly HttpClient _userClient;
    private readonly HttpClient _anonymusClient;

    public DeskControllerTests(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
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
    public async Task GetAllDesks_WithAdminToken_ShouldReturnOkAndDesksList()
    {
        // ACT
        var response = await _adminClient.GetAsync("/api/Desk");
        
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var desks = response.Content.ReadFromJsonAsync<List<DeskDto>>();
        
        desks.Should().NotBeNull();
        desks.Result.First().Id.Should().Be(1);


    }
    
    [Fact]
    public async Task GetAllUsers_WithUserToken_ShouldReturnForbidden()
    {
        // ACT
        var response = await _userClient.GetAsync("/api/User");
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetDeskById_ShouldReturnDeskDto()
    {
        // ACT
        var response = await _userClient.GetAsync("/api/Desk/1");
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var desk = await response.Content.ReadFromJsonAsync<DeskDto>();
        desk.Should().NotBeNull();
        desk.Id.Should().Be(1);
        desk.Name.Should().Be("testDesk");
        desk.DeskType.Should().Be("Vip");
    }

    [Fact]
    public async Task GetDeskById_ShouldReturnNotFound()
    {
        // ACT
        var response = await _adminClient.GetAsync("api/Desk/9999");
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
}