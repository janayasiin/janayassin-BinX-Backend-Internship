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

        modelBuilder.Entity<Event>().HasData(
    new Event
    {
        Id = 1,
        Title = "Tech Conference 2026",
        Description = "A conference about modern technology and software development.",
        Location = "Nablus",
        StartDate = new DateTime(2026, 9, 10, 10, 0, 0),
        EndDate = new DateTime(2026, 9, 10, 17, 0, 0),
        Capacity = 200,
        CreatedAt = new DateTime(2026, 8, 1),
        CategoryId = 2
    },
    new Event
    {
        Id = 2,
        Title = "Live Music Night",
        Description = "An evening featuring live music performances.",
        Location = "Ramallah",
        StartDate = new DateTime(2026, 9, 15, 18, 0, 0),
        EndDate = new DateTime(2026, 9, 15, 22, 0, 0),
        Capacity = 150,
        CreatedAt = new DateTime(2026, 8, 1),
        CategoryId = 1
    },
    new Event
    {
        Id = 3,
        Title = "Football Championship",
        Description = "A local football championship event.",
        Location = "Nablus",
        StartDate = new DateTime(2026, 9, 20, 16, 0, 0),
        EndDate = new DateTime(2026, 9, 20, 20, 0, 0),
        Capacity = 500,
        CreatedAt = new DateTime(2026, 8, 2),
        CategoryId = 3
    },
    new Event
    {
        Id = 4,
        Title = "Developer Meetup",
        Description = "A meetup for developers to discuss backend and web development.",
        Location = "Nablus",
        StartDate = new DateTime(2026, 10, 5, 11, 0, 0),
        EndDate = new DateTime(2026, 10, 5, 15, 0, 0),
        Capacity = 100,
        CreatedAt = new DateTime(2026, 8, 2),
        CategoryId = 2
    },
    new Event
    {
        Id = 5,
        Title = "Summer Music Festival",
        Description = "A large outdoor music festival.",
        Location = "Ramallah",
        StartDate = new DateTime(2026, 10, 12, 17, 0, 0),
        EndDate = new DateTime(2026, 10, 12, 23, 0, 0),
        Capacity = 1000,
        CreatedAt = new DateTime(2026, 8, 3),
        CategoryId = 1
    }
);
    }
}