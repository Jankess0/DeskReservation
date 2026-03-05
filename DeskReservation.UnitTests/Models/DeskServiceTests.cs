using AutoMapper;
using DeskReservation.DbContext;
using DeskReservation.DTOs;
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
        
        var user = new User
        {
            Id = 1,
            Email = "mock@email.com",
            PasswordHash = "password",
            FirstName = "Name",
            LastName = "Last",
            Role = UserRole.User
        };
        dbContext.Users.Add(user);

        var booking = new Booking
        {
            Id = 1,
            UserId = 1,
            DeskId = 1
        };
        dbContext.Bookings.Add(booking);
        
        await dbContext.SaveChangesAsync();
        
        var service = new DeskService(dbContext, mapperMock.Object, observers);
        
        // ACT
        var result = await service.CheckOutAsync(1, 1);
        
        // ASSERT
        result.Should().BeTrue();
        
        var updatedDesk = await dbContext.Desks.FindAsync(1);
        updatedDesk.Status.Should().Be(DeskState.Cleaning);
        
        observerMock.Verify(o => o.Update(It.IsAny<Desk>()), Times.Once);
    }
    

    [Fact]
    public async Task CheckIn_WhenDeskExistsAndUserHasPermission_ShouldChangeState()
    {
        // ARRANGE
        var dbContext = GetInMemoryDbContext(Guid.NewGuid().ToString());
        var mapperMock = new Mock<IMapper>();
        var observers = new List<IObserver>();
        var service = new DeskService(dbContext, mapperMock.Object, observers);

        var desk = new Desk
        {
            Id = 1,
            Name = "Desk 1",
            Status = DeskState.Available,
            IsAdminOnly = false
        };
        
        dbContext.Desks.Add(desk);
        

        var user = new User
        {
            Id = 1,
            Email = "mock@email.com",
            PasswordHash = "password",
            FirstName = "Name",
            LastName = "Last",
            Role = UserRole.User
        };
        
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        
        
        // ACT
        var result = await service.CheckInAsync(1, 1);
        
        // ASSERT
        result.Should().BeTrue();
        var updatedDesk = await dbContext.Desks.FindAsync(1);
        updatedDesk.Status.Should().Be(DeskState.Occupied);
    }
    
    [Fact]
    public async Task CheckIn_WhenDeskExistsAndUserHasNotPermission_ShouldThrowException()
    {
        // ARRANGE
        var dbContext = GetInMemoryDbContext(Guid.NewGuid().ToString());
        var mapperMock = new Mock<IMapper>();
        var observers = new List<IObserver>();
        var service = new DeskService(dbContext, mapperMock.Object, observers);

        var desk = new Desk
        {
            Id = 1,
            Name = "Desk 1",
            Status = DeskState.Available,
            IsAdminOnly = true
        };
        
        dbContext.Desks.Add(desk);

        var user = new User
        {
            Id = 1,
            Email = "mock@email.com",
            PasswordHash = "password",
            FirstName = "Name",
            LastName = "Last",
            Role = UserRole.User
        };
        
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        
        
        // ACT
        Func<Task> act = async () => await service.CheckInAsync(1, 1);
        
        // ASSERT
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("No permission to check in desk");
        
    }

    [Fact]
    public async Task CheckIn_WhenUserNotFound_ShouldThrowException()
    {
        // ARRANGE
        var dbContext = GetInMemoryDbContext(Guid.NewGuid().ToString());
        var mapperMock = new Mock<IMapper>();
        var observers = new List<IObserver>();
        var service = new DeskService(dbContext, mapperMock.Object, observers);

        var desk = new Desk
        {
            Id = 1,
            Name = "Desk 1",
            Status = DeskState.Available,
            IsAdminOnly = true
        };
        dbContext.Desks.Add(desk);
        await dbContext.SaveChangesAsync();
        
        // ACT
        Func<Task> act = async () => await service.CheckInAsync(1, 1);
        
        // ASSERT
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("User not found");
    }
    
    [Fact]
    public async Task CheckIn_WhenDeskNotFound_ShouldThrowException()
    {
        // ARRANGE
        var dbContext = GetInMemoryDbContext(Guid.NewGuid().ToString());
        var mapperMock = new Mock<IMapper>();
        var observers = new List<IObserver>();
        var service = new DeskService(dbContext, mapperMock.Object, observers);

        var user = new User
        {
            Id = 1,
            Email = "mock@email.com",
            PasswordHash = "password",
            FirstName = "Name",
            LastName = "Last",
            Role = UserRole.User
        };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        
        // ACT
        Func<Task> act = async () => await service.CheckInAsync(1, 1);
        
        // ASSERT
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Desk not found");
    }
    
    [Fact]
    public async Task GetDeskAsync_WhenDeskIsCleaningAndTimePassed_ShouldReturnDTOStateAvailble()
    {
        // ARRANGE
        var dbContext = GetInMemoryDbContext(Guid.NewGuid().ToString());
        var mapperMock = new Mock<IMapper>();
        var observers = new List<IObserver>();
        var service = new DeskService(dbContext, mapperMock.Object, observers);

        var desk = new Desk
        {
            Id = 1,
            Name = "Desk 1",
            IsAdminOnly = false,
            LastStatusChangeDate = DateTime.UtcNow.AddHours(-2),
            Status = DeskState.Cleaning
        };
        dbContext.Desks.Add(desk);
        await dbContext.SaveChangesAsync();

        var fakeDto = new DeskDto { Id = 1, Status = "Cleaning" };
        mapperMock.Setup(x => x.Map<DeskDto>(It.IsAny<Desk>())).Returns(fakeDto);
        
        // ACT
        var result = await service.GetDeskAsync(1);
        
        // ASSERT
        result.Should().NotBeNull();
        result.Status.Should().Be("Available");
    }
    
    [Fact]
    public async Task GetDeskAsync_WhenDeskIsCleaningAndTimeNotPassed_ShouldReturnDTOStateCleaning()
    {
        // ARRANGE
        var dbContext = GetInMemoryDbContext(Guid.NewGuid().ToString());
        var mapperMock = new Mock<IMapper>();
        var observers = new List<IObserver>();
        var service = new DeskService(dbContext, mapperMock.Object, observers);

        var desk = new Desk
        {
            Id = 1,
            Name = "Desk 1",
            IsAdminOnly = false,
            LastStatusChangeDate = DateTime.UtcNow,
            Status = DeskState.Cleaning
        };
        dbContext.Desks.Add(desk);
        await dbContext.SaveChangesAsync();

        var fakeDto = new DeskDto { Id = 1, Status = "Cleaning" };
        mapperMock.Setup(x => x.Map<DeskDto>(It.IsAny<Desk>())).Returns(fakeDto);
        
        // ACT
        var result = await service.GetDeskAsync(1);
        
        // ASSERT
        result.Should().NotBeNull();
        result.Status.Should().Be("Cleaning 1 minutes");
    }
}