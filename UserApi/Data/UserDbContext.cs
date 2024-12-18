using Microsoft.EntityFrameworkCore;
using UserApi.Entities;

namespace UserApi.Data;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserChild> UserChildren { get; set; }
    public DbSet<UserParent> UserParents { get; set; }

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<UserParent>()
            .HasOne(up => up.User)
            .WithOne(u => u.UserParent)
            .HasForeignKey<UserParent>(up => up.UserId);

        modelBuilder.Entity<UserChild>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserChildren)
            .HasForeignKey(uc => uc.UserId);
    }
}
