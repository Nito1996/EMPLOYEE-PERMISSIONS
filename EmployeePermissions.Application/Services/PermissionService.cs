using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeePermissions.Application.DTOs;
using EmployeePermissions.Domain.Entities;
using EmployeePermissions.Domain.Repositories;

namespace EmployeePermissions.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionTypeRepository _permissionTypeRepository;

        public PermissionService(IPermissionRepository permissionRepository, IPermissionTypeRepository permissionTypeRepository)
        {
            _permissionRepository = permissionRepository;
            _permissionTypeRepository = permissionTypeRepository;
        }

        public async Task<PermissionDto> GetByIdAsync(Guid id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null) return null;

            return new PermissionDto
            {
                PermissionId = permission.PermissionId,
                EmployeeFirstName = permission.EmployeeFirstName,
                EmployeeLastName = permission.EmployeeLastName,
                PermissionTypeId = permission.PermissionTypeId,
                PermissionDescription = permission.PermissionType?.PermissionDescription,
                PermissionDate = permission.PermissionDate
            };
        }

        public async Task<IEnumerable<PermissionDto>> GetAllAsync()
        {
            var permissions = await _permissionRepository.GetAllAsync();
            return permissions.Select(p => new PermissionDto
            {
                PermissionId = p.PermissionId,
                EmployeeFirstName = p.EmployeeFirstName,
                EmployeeLastName = p.EmployeeLastName,
                PermissionTypeId = p.PermissionTypeId,
                PermissionDescription = p.PermissionType?.PermissionDescription,
                PermissionDate = p.PermissionDate
            }).ToList();
        }

        public async Task<PermissionDto> CreateAsync(CreateEmployeePermissionDto createPermissionDto)
        {
            var permissionType = await _permissionTypeRepository.GetByDescriptionAsync(createPermissionDto.PermissionDescription);
            if (permissionType == null)
            {
                permissionType = new PermissionType
                {
                    PermissionDescription = createPermissionDto.PermissionDescription
                };
                await _permissionTypeRepository.AddAsync(permissionType);
            }

            var permission = new Permission
            {
                EmployeeFirstName = createPermissionDto.EmployeeFirstName,
                EmployeeLastName = createPermissionDto.EmployeeLastName,
                PermissionType = permissionType,
                PermissionDate = DateTimeOffset.UtcNow
            };

            await _permissionRepository.AddAsync(permission);

            return new PermissionDto
            {
                PermissionId = permission.PermissionId,
                EmployeeFirstName = permission.EmployeeFirstName,
                EmployeeLastName = permission.EmployeeLastName,
                PermissionTypeId = permission.PermissionTypeId,
                PermissionDescription = permissionType.PermissionDescription,
                PermissionDate = permission.PermissionDate
            };
        }

        public async Task UpdateAsync(Guid id, UpdateEmployeePermissionDto updatePermissionDto)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null) throw new KeyNotFoundException("Permission not found");

            permission.EmployeeFirstName = updatePermissionDto.EmployeeFirstName;
            permission.EmployeeLastName = updatePermissionDto.EmployeeLastName;

            var permissionType = await _permissionTypeRepository.GetByDescriptionAsync(updatePermissionDto.PermissionDescription);
            if (permissionType == null)
            {
                permissionType = new PermissionType
                {
                    PermissionDescription = updatePermissionDto.PermissionDescription
                };
                await _permissionTypeRepository.AddAsync(permissionType);
            }

            permission.PermissionType = permissionType;
            await _permissionRepository.UpdateAsync(permission);
        }

        public async Task DeleteAsync(Guid id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null) throw new KeyNotFoundException("Permission not found");

            await _permissionRepository.DeleteAsync(id);
        }
    }
}