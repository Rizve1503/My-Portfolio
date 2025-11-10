using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RizvePortfolio.Domain.Entities;
using RizvePortfolio.Infrastructure.Identity;

namespace RizvePortfolio.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<PortfolioItem> PortfolioItems => Set<PortfolioItem>();
    
    // New entities
    public DbSet<Visitor> Visitors => Set<Visitor>();
    public DbSet<CvVersion> CvVersions => Set<CvVersion>();
    public DbSet<CvDownload> CvDownloads => Set<CvDownload>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Category>(b =>
        {
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.HasMany(x => x.Items)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<PortfolioItem>(b =>
        {
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
        });

        // New entity configurations
        builder.Entity<Visitor>(b =>
        {
            b.HasKey(x => x.VisitorId);
            b.Property(x => x.IpHash).HasMaxLength(64).IsRequired();
            b.Property(x => x.Path).HasMaxLength(500).IsRequired();
        });

        builder.Entity<CvVersion>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.FileName).HasMaxLength(255).IsRequired();
            b.Property(x => x.FilePath).HasMaxLength(500).IsRequired();
        });

        builder.Entity<CvDownload>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasOne(x => x.CvVersion)
                .WithMany(x => x.CvDownloads)
                .HasForeignKey(x => x.CvVersionId)
                .OnDelete(DeleteBehavior.Restrict);
            b.HasOne(x => x.Visitor)
                .WithMany(x => x.CvDownloads)
                .HasForeignKey(x => x.VisitorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Project>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.Slug).HasMaxLength(200).IsRequired();
            b.HasIndex(x => x.Slug).IsUnique();
        });

        builder.Entity<Skill>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).HasMaxLength(100).IsRequired();
            b.Property(x => x.Category).HasMaxLength(100).IsRequired();
        });

        builder.Entity<Service>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
        });

        builder.Entity<Experience>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Company).HasMaxLength(200).IsRequired();
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
        });

        builder.Entity<Education>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Institution).HasMaxLength(200).IsRequired();
            b.Property(x => x.Degree).HasMaxLength(200).IsRequired();
        });

        builder.Entity<Contact>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.Email).HasMaxLength(200).IsRequired();
        });
    }
}
