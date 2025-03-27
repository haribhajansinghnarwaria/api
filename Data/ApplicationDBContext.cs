using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>  // using this  IdentityDbContext<AppUser>  in place of DBConetxt to use Identity and it is also inherit DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Stock> Stock { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Stock)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.StockId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<IdentityUserLogin<string>>()
      .HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>()
 .HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>()
 .HasNoKey();

            //Many to many relations for Portfolio start
            modelBuilder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));
            modelBuilder.Entity<Portfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);//This will create one to many relations between AppUser and Portfolios

            modelBuilder.Entity<Portfolio>()
               .HasOne(u => u.Stock)
               .WithMany(u => u.Portfolios)
               .HasForeignKey(p => p.StockId); //This will create one to many relationship between Stock and Portfolios
            // Many to many relations for Portfolio end

            //Creating the roles
            base.OnModelCreating(modelBuilder);
            List<IdentityRole> roles = new List<IdentityRole> { 
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);//This line will insert data in AspNetRoles tables


        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)); // this line is to add to suppress this error
            //An error was generated for warning 'Microsoft.EntityFrameworkCore.Migrations.PendingModelChangesWarning': The model for context 'ApplicationDBContext' changes each time it is built. This is usually caused by dynamic values used in a 'HasData' call (e.g. `new DateTime()`, `Guid.NewGuid()`). Add a new migration and examine its contents to locate the cause, and replace the dynamic call with a static, hardcoded value. See https://aka.ms/efcore-docs-pending-changes. This exception can be suppressed or logged by passing event ID 'RelationalEventId.PendingModelChangesWarning' to the 'ConfigureWarnings' method in 'DbContext.OnConfiguring' or 'AddDbContext'.
        }
    }
}
