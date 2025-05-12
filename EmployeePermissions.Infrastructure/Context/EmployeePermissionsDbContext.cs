using EmployeePermissions.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeePermissions.Infrastructure.Context
{
    public class EmployeePermissionsDbContext : DbContext
    {
        public EmployeePermissionsDbContext(DbContextOptions<EmployeePermissionsDbContext> options) : base(options) { }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>()
                .HasKey(p => p.PermissionId);

            modelBuilder.Entity<PermissionType>()
                .HasKey(pt => pt.PermissionTypeId);

            modelBuilder.Entity<PermissionType>()
                .HasMany(pt => pt.Permissions)
                .WithOne(p => p.PermissionType)
                .HasForeignKey(p => p.PermissionTypeId);

            base.OnModelCreating(modelBuilder);
        }
    }
}