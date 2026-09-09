using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<VitalSign> VitalSigns { get; set; }
    public DbSet<Medication> Medications { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<MedicalCondition> MedicalConditions { get; set; }
    public DbSet<AppointmentNote> AppointmentNotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ApplicationUser -> Patient (One-to-Zero/One)
        modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.Patient)
            .WithOne(p => p.User)
            .HasForeignKey<Patient>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Patient -> VitalSigns
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.VitalSigns)
            .WithOne(v => v.Patient)
            .HasForeignKey(v => v.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        // Patient -> Medications
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Medications)
            .WithOne(m => m.Patient)
            .HasForeignKey(m => m.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        // Patient -> Appointments
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Appointments)
            .WithOne(a => a.Patient)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index for frequent UserId lookups
        modelBuilder.Entity<Patient>()
            .HasIndex(p => p.UserId);

        // Appointment -> AppointmentNote
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Note)
            .WithOne(n => n.Appointment)
            .HasForeignKey<AppointmentNote>(n => n.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // AppointmentNote configuration
        modelBuilder.Entity<AppointmentNote>()
            .Property(n => n.Note)
            .IsRequired()
            .HasMaxLength(50);
        // Appointment index for duplicate appointment checks
        modelBuilder.Entity<Appointment>()
            .HasIndex(a => new { a.PatientId, a.AppointmentDate });
        // VitalSign index
        modelBuilder.Entity<VitalSign>()
            .HasIndex(v => new { v.PatientId, v.RecordedAt });

        // Decimal precision
        modelBuilder.Entity<VitalSign>()
            .Property(v => v.Temperature)
            .HasColumnType("decimal(5,2)");

        // MedicalCondition configuration
        modelBuilder.Entity<MedicalCondition>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<MedicalCondition>()
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<MedicalCondition>()
            .Property(c => c.Description)
            .HasMaxLength(500);

        // MedicalCondition seed data
        modelBuilder.Entity<MedicalCondition>()
            .HasData(
                new MedicalCondition
                {
                    Id = 1,
                    Name = "Hypertension",
                    Description = "High blood pressure"
                },
                new MedicalCondition
                {
                    Id = 2,
                    Name = "High Cholesterol",
                    Description = "Elevated cholesterol levels"
                },
                new MedicalCondition
                {
                    Id = 3,
                    Name = "Coronary Artery Disease",
                    Description = "Disease affecting the coronary arteries"
                }
            );
    }
}