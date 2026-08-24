using EventHub.Models;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<TicketType> TicketTypes { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingItem> BookingItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // User
        // =========================
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.CreatedAt)
                .IsRequired();
        });

        // =========================
        // Category
        // =========================
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(c => c.Name)
                .IsUnique();

            entity.Property(c => c.Description)
                .HasMaxLength(500);
        });

        // =========================
        // Event
        // =========================
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.StartDate)
                .IsRequired();

            entity.Property(e => e.EndDate)
                .IsRequired();

            entity.Property(e => e.Capacity)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            // Category 1 -> Many Events
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // =========================
        // TicketType
        // =========================
        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(t => t.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            entity.Property(t => t.QuantityAvailable)
                .IsRequired();

            // Event 1 -> Many TicketTypes
            entity.HasOne(t => t.Event)
                .WithMany(e => e.TicketTypes)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // =========================
        // Booking
        // =========================
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(b => b.Id);

            entity.Property(b => b.BookingDate)
                .IsRequired();

            entity.Property(b => b.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(b => b.TotalAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            // User 1 -> Many Bookings
            entity.HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // =========================
        // BookingItem
        // =========================
        modelBuilder.Entity<BookingItem>(entity =>
        {
            entity.HasKey(bi => bi.Id);

            entity.Property(bi => bi.Quantity)
                .IsRequired();

            entity.Property(bi => bi.UnitPrice)
                .IsRequired()
                .HasPrecision(18, 2);

            // Booking 1 -> Many BookingItems
            entity.HasOne(bi => bi.Booking)
                .WithMany(b => b.BookingItems)
                .HasForeignKey(bi => bi.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // TicketType 1 -> Many BookingItems
            entity.HasOne(bi => bi.TicketType)
                .WithMany(t => t.BookingItems)
                .HasForeignKey(bi => bi.TicketTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // =========================
        // Seed Data
        // =========================
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Music",
                Description = "Music and live performance events"
            },
            new Category
            {
                Id = 2,
                Name = "Technology",
                Description = "Technology conferences and events"
            },
            new Category
            {
                Id = 3,
                Name = "Sports",
                Description = "Sports events and competitions"
            }
        );
    }
}