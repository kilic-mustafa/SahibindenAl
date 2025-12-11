using SahibindenAl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SahibindenAl.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Advert> Adverts { get; set; }
    public DbSet<AdvertImage> AdvertImages { get; set; }
    public DbSet<CategoryPropertyKey> CategoryPropertyKeys { get; set; }
    public DbSet<CategoryPropertyOption> CategoryPropertyOptions { get; set; }
    public DbSet<AdvertPropertyValue> AdvertPropertyValues { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Favorite> Favorites { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");
        
        modelBuilder.Entity<Category>()
            .HasOne(x => x.ParentCategory)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Advert>()
            .Property(x => x.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Advert>()
            .HasMany(a => a.AdvertImages)
            .WithOne(i => i.Advert)
            .HasForeignKey(i => i.AdvertId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Advert>()
            .HasMany(a => a.AdvertPropertyValues)
            .WithOne(v => v.Advert)
            .HasForeignKey(v => v.AdvertId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Advert>()
            .HasOne(a => a.Category)
            .WithMany(c => c.Adverts)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Advert>()
            .HasOne(a => a.User)
            .WithMany(u => u.Adverts)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AdvertPropertyValue>()
            .HasOne(v => v.PropertyKey)
            .WithMany()
            .HasForeignKey(v => v.CategoryPropertyKeyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<District>()
            .HasOne(d => d.City)
            .WithMany(c => c.Districts)
            .HasForeignKey(d => d.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Favorite>()
            .HasKey(f => new { f.UserId, f.AdvertId });

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId);

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.Advert)
            .WithMany(a => a.Favorites)
            .HasForeignKey(f => f.AdvertId);

        modelBuilder.Entity<CategoryPropertyOption>()
            .HasOne(po => po.CategoryPropertyKey)
            .WithMany(cpk => cpk.CategoryPropertyOptions)
            .HasForeignKey(po => po.CategoryPropertyKeyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}