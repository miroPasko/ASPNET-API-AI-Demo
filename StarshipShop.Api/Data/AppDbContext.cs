using Microsoft.EntityFrameworkCore;
using StarshipShop.Api.Models;

namespace StarshipShop.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<PaymentDetails> PaymentDetails { get; set; } = null!;
    public DbSet<Engine> Engines { get; set; } = null!;
    public DbSet<FtlDrive> FtlDrives { get; set; } = null!;
    public DbSet<Starship> Starships { get; set; } = null!;
    public DbSet<PrivateVessel> PrivateVessels { get; set; } = null!;
    public DbSet<PublicTransportVessel> PublicTransportVessels { get; set; } = null!;
    public DbSet<CargoVessel> CargoVessels { get; set; } = null!;
    public DbSet<Sale> Sales { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure TPT (Table-Per-Type) inheritance strategy for Starship hierarchy
        modelBuilder.Entity<Starship>()
            .UseTptMappingStrategy();

        // Configure unique index on User.Email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Configure enum-to-string conversions for PrivateVessel
        modelBuilder.Entity<PrivateVessel>()
            .Property(pv => pv.VesselType)
            .HasConversion<string>();

        // Configure enum-to-string conversions for PublicTransportVessel
        modelBuilder.Entity<PublicTransportVessel>()
            .Property(ptv => ptv.TransportClass)
            .HasConversion<string>();

        // Configure enum-to-string conversions for CargoVessel
        modelBuilder.Entity<CargoVessel>()
            .Property(cv => cv.CargoType)
            .HasConversion<string>();

        // Configure decimal precision for Price
        modelBuilder.Entity<Starship>()
            .Property(s => s.Price)
            .HasPrecision(18, 2);

        // Configure relationships with delete behaviors
        
        // Starship → Engine: Restrict
        modelBuilder.Entity<Starship>()
            .HasOne(s => s.Engine)
            .WithMany(e => e.Starships)
            .HasForeignKey(s => s.EngineId)
            .OnDelete(DeleteBehavior.Restrict);

        // Starship → FtlDrive: SetNull
        modelBuilder.Entity<Starship>()
            .HasOne(s => s.FtlDrive)
            .WithMany(f => f.Starships)
            .HasForeignKey(s => s.FtlDriveId)
            .OnDelete(DeleteBehavior.SetNull);

        // Sale → User: Cascade
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.User)
            .WithMany(u => u.Sales)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Sale → Starship: Restrict
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Starship)
            .WithMany(st => st.Sales)
            .HasForeignKey(s => s.StarshipId)
            .OnDelete(DeleteBehavior.Restrict);

        // Sale → PaymentDetails: Restrict
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.PaymentDetails)
            .WithMany(pd => pd.Sales)
            .HasForeignKey(s => s.PaymentDetailsId)
            .OnDelete(DeleteBehavior.Restrict);

        // User → PaymentDetails relationship
        modelBuilder.Entity<PaymentDetails>()
            .HasOne(pd => pd.User)
            .WithMany(u => u.PaymentDetails)
            .HasForeignKey(pd => pd.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
