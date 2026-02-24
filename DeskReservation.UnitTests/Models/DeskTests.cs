using AutoMapper;
using DeskReservation.DbContext;
using DeskReservation.Models;
using DeskReservation.Observer;
using DeskReservation.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DeskReservation.UnitTests.Models;

public class DeskServiceTests
{
    private AppDbContext GetInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CheckOut_WhenDeskExists_ShouldChangeStateAndNotify()
    {
        // ARRANGE
        var dbContext = GetInMemoryDbContext(Guid.NewGuid().ToString());
        var mapperMock = new Mock<IMapper>();

        var observerMock = new Mock<IObserver>();
        var observers = new List<IObserver>();
        observers.Add(observerMock.Object);

        var desk = new Desk
        {
            Id = 1,
            Name = "Desk 1",
            Status = DeskState.Occupied,
            IsAdminOnly = false
        };
        dbContext.Desks.Add(desk);
        await dbContext.SaveChangesAsync();
        
        var service = new DeskService(dbContext, mapperMock.Object, observers);
        
        // ACT
        var result = await service.CheckOutAsync(1);
        
        // ASSERT
        result.Should().BeTrue();
        
        var updatedDesk = await dbContext.Desks.FindAsync(1);
        updatedDesk.Status.Should().Be(DeskState.Cleaning);
        
        observerMock.Verify(o => o.Update(It.IsAny<Desk>()), Times.Once);
    }
    
    [Fact]
    public async Task CheckOut_WhenDeskDoesNotExist_ShouldThrowException()
    {
        // ARRANGE
        var dbContext = GetInMemoryDbContext(Guid.NewGuid().ToString());
        var service = new DeskService(dbContext, null, null);
        
        // ACT
        Func<Task> act = async () => await service.CheckOutAsync(999);
        
        // ASSERT
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Desk not found");
        
    }
}