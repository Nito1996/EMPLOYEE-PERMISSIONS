using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeePermissions.Domain.Entities;
using EmployeePermissions.Domain.Repositories;
using EmployeePermissions.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeePermissions.Infrastructure.Repositories
{
    public class PermissionTypeRepository : IPermissionTypeRepository
    {
        private readonly EmployeePermissionsDbContext _dbContext;

        public PermissionTypeRepository(EmployeePermissionsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PermissionType> GetByIdAsync(Guid id)
        {
            return await _dbContext.PermissionTypes.FindAsync(id);
        }

        public async Task<IEnumerable<PermissionType>> GetAllAsync()
        {
            return await _dbContext.PermissionTypes.ToListAsync();
        }

        public async Task AddAsync(PermissionType permissionType)
        {
            _dbContext.PermissionTypes.Add(permissionType);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(PermissionType permissionType)
        {
            _dbContext.PermissionTypes.Update(permissionType);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var permissionType = await _dbContext.PermissionTypes.FindAsync(id);
            if (permissionType != null)
            {
                _dbContext.PermissionTypes.Remove(permissionType);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<PermissionType> GetByDescriptionAsync(string description)
        {
            return await _dbContext.PermissionTypes
                .FirstOrDefaultAsync(pt => pt.PermissionDescription == description);
        }
    }
}