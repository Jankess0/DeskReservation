using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;
using Microsoft.EntityFrameworkCore;
using DeskReservation.DbContext;
using DeskReservation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DeskReservation.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();
            var testDesk = new Desk { Id = 1, Name = "testDesk", IsAdminOnly = true };
            dbContext.Desks.Add(testDesk);
            dbContext.SaveChangesAsync();
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TestUser";
                options.DefaultChallengeScheme = "TestUser";
            }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestUser", options => { })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestAdmin", options => { });
        });
    }
}