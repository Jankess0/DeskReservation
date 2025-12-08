using DeskReservation.Models;
using Microsoft.EntityFrameworkCore;

namespace DeskReservation.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Desk> Desks { get; set; }
    public DbSet<Floor> Floors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Room>()
            .HasOne(f => f.Floor)
            .WithMany(r => r.Rooms)
            .HasForeignKey(f => f.FloorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Desk>()
            .HasOne(r => r.Room)
            .WithMany(r => r.Desks)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}