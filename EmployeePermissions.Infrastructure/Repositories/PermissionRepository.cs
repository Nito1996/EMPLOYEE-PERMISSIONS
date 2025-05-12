using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeePermissions.Domain.Entities;
using EmployeePermissions.Domain.Repositories;
using EmployeePermissions.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeePermissions.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly EmployeePermissionsDbContext _dbContext;

        public PermissionRepository(EmployeePermissionsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Permission> GetByIdAsync(Guid id)
        {
            return await _dbContext.Permissions
                .Include(p => p.PermissionType)
                .FirstOrDefaultAsync(p => p.PermissionId == id);
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _dbContext.Permissions
                .Include(p => p.PermissionType)
                .ToListAsync();
        }

        public async Task AddAsync(Permission permission)
        {
            _dbContext.Permissions.Add(permission);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Permission permission)
        {
            _dbContext.Permissions.Update(permission);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var permission = await _dbContext.Permissions.FindAsync(id);
            if (permission != null)
            {
                _dbContext.Permissions.Remove(permission);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}