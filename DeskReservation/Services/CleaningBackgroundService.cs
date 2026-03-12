using DeskReservation.DbContext;
using DeskReservation.Models;
using Microsoft.EntityFrameworkCore;

namespace DeskReservation.Services;

public class CleaningBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CleaningBackgroundService> _logger;
    
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(30); 
    private const int CleaningTimeMinutes = 1;

    public CleaningBackgroundService(IServiceProvider serviceProvider, ILogger<CleaningBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Cleaning background service");
        
        using var timer = new PeriodicTimer(_checkInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await ProcessCleaningDesksAsync(stoppingToken);
        }
    }

    private async Task ProcessCleaningDesksAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var desksToClean = await context.Desks
            .Where(d => d.Status == DeskState.Cleaning)
            .ToListAsync(stoppingToken);
        
        if (!desksToClean.Any()) return;
        
        var now = DateTime.UtcNow;
        bool anyDeskUpdated = false;

        foreach (var desk in desksToClean)
        {
            var timeElapsed = now - desk.LastStatusChangeDate;

            if (timeElapsed.TotalMinutes >= CleaningTimeMinutes)
            {
                desk.Status = DeskState.Available;
                desk.LastStatusChangeDate = now; 
                anyDeskUpdated = true;
                
                _logger.LogInformation($"BackgroundService: Desk ID {desk.Id} finished cleaning and is now Available.");
            }
        }
        if (anyDeskUpdated)
        {
            await context.SaveChangesAsync(stoppingToken);
        }
        
    }
}