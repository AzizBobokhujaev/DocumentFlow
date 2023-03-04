using Api.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.DbContext;

public class DocumentFlowDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DocumentFlowDbContext(DbContextOptions options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    public DbSet<Order> Orders { get; set; }
    public override DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<User>(entity => entity.ToTable("Users"));
        builder.Entity<IdentityRole<int>>(entity => entity.ToTable("Roles"));
        builder.Entity<IdentityUserRole<int>>(entity => entity.ToTable("UserRoles"));
        builder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable("UserClaims"));
        builder.Entity<IdentityUserLogin<int>>(entity => entity.ToTable("UserLogins"));
        builder.Entity<IdentityUserToken<int>>(entity => entity.ToTable("UserTokens"));
        builder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable("RoleClaims"));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLazyLoadingProxies();
    }
}