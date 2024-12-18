using EmployeeAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Data;

public class EmployeeDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }

    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().HasKey(e => e.Id);

        modelBuilder.Entity<Employee>()
           .HasIndex(e => e.Department)
           .HasDatabaseName("IX_Employee_Department");

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Name)
            .HasDatabaseName("IX_Employee_Name");
    }
}