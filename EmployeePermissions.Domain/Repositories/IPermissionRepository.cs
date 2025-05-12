using EmployeePermissions.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EmployeePermissions.Domain.Repositories
{
    public interface IPermissionRepository
    {
        Task<Permission> GetByIdAsync(Guid id);
        Task<IEnumerable<Permission>> GetAllAsync();
        Task AddAsync(Permission permission);
        Task UpdateAsync(Permission permission);
        Task DeleteAsync(Guid id);
    }
}
