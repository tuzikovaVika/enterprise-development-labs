using AirlineBooking.Domain.Data;
using AirlineBooking.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace AirlineBooking.Infrastructure.EfCore;

/// <summary>
///     Контекст базы данных для управления сущностями системы бронирования авиабилетов.
/// </summary>
public class AirlineBookingDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    /// <summary>
    ///     Настройка модели базы данных.
    /// </summary>
    /// <param name="modelBuilder">Инструмент для настройки модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flight>(builder =>
        {
            builder.HasKey(f => f.Id);
            builder.HasMany(f => f.Bookings).WithOne(b => b.Flight).IsRequired(false);
            builder.HasData(DataSeeder.Flights);
        });

        modelBuilder.Entity<Customer>(builder =>
        {
            builder.HasKey(c => c.Id);
            builder.HasMany(c => c.Bookings).WithOne(b => b.Customer).IsRequired(false);
            builder.HasData(DataSeeder.Customers);
        });

        modelBuilder.Entity<Booking>(builder =>
        {
            builder.HasKey(b => b.Id);
            builder.HasOne(b => b.Customer).WithMany(c => c.Bookings).HasForeignKey(b => b.CustomerId).IsRequired();
            builder.HasOne(b => b.Flight).WithMany(f => f.Bookings).HasForeignKey(b => b.FlightId).IsRequired();
            builder.HasData(DataSeeder.Bookings);
        });
    }
}