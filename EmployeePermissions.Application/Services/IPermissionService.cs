using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeePermissions.Application.DTOs;

namespace EmployeePermissions.Application.Services
{
    public interface IPermissionService
    {
        Task<PermissionDto> GetByIdAsync(Guid id);
        Task<IEnumerable<PermissionDto>> GetAllAsync();
        Task<PermissionDto> CreateAsync(CreateEmployeePermissionDto createPermissionDto);
        Task UpdateAsync(Guid id, UpdateEmployeePermissionDto updatePermissionDto);
        Task DeleteAsync(Guid id);
    }
}