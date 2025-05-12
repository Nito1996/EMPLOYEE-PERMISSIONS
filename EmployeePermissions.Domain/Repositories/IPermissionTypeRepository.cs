using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeePermissions.Domain.Entities;

namespace EmployeePermissions.Domain.Repositories
{
    public interface IPermissionTypeRepository
    {
        Task<PermissionType> GetByIdAsync(Guid id);
        Task<IEnumerable<PermissionType>> GetAllAsync();
        Task AddAsync(PermissionType permissionType);
        Task UpdateAsync(PermissionType permissionType);
        Task DeleteAsync(Guid id);
        Task<PermissionType> GetByDescriptionAsync(string description);
    }
}
