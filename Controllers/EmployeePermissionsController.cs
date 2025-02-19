using System;
using System.Linq;
using System.Threading.Tasks;
using EmployeePermissions.Application.DTOs;
using EmployeePermissions.Domain.Entities;
using EmployeePermissions.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeePermissions.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePermissionsController : ControllerBase
    {
        private readonly EmployeePermissionsDbContext _dbContext;
        private readonly ILogger<EmployeePermissionsController> _logger;

        public EmployeePermissionsController(EmployeePermissionsDbContext dbContext, ILogger<EmployeePermissionsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeePermission([FromBody] CreateEmployeePermissionDto newPermission)
        {
            if (newPermission == null || string.IsNullOrEmpty(newPermission.PermissionDescription)) return BadRequest("Invalid request. Permission data is required.");

            try
            {
                var existingPermissionType = await _dbContext.PermissionTypes
                    .FirstOrDefaultAsync(pt => pt.PermissionDescription == newPermission.PermissionDescription);

                if (existingPermissionType == null)
                {
                    existingPermissionType = new PermissionType
                    {
                        PermissionDescription = newPermission.PermissionDescription
                    };
                    _dbContext.PermissionTypes.Add(existingPermissionType);
                    await _dbContext.SaveChangesAsync();
                }

                var permissionToAdd = new Permission
                {
                    EmployeeFirstName = newPermission.EmployeeFirstName,
                    EmployeeLastName = newPermission.EmployeeLastName,
                    PermissionType = existingPermissionType,
                    PermissionDate = DateTimeOffset.UtcNow
                };

                _dbContext.Permissions.Add(permissionToAdd);
                await _dbContext.SaveChangesAsync(); 

                return StatusCode(201, new
                {
                    permissionToAdd.PermissionId,
                    permissionToAdd.EmployeeFirstName,
                    permissionToAdd.EmployeeLastName,
                    permissionToAdd.PermissionTypeId,
                    PermissionDescription = existingPermissionType.PermissionDescription,
                    permissionToAdd.PermissionDate
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new permission.");
                return StatusCode(500, "An error occurred. Please try again later.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeePermissions()
        {
            try
            {
                var results = await _dbContext.Permissions
                              .OrderBy(p => p.PermissionId)
                              .Select(p => new PermissionDto
                              {
                                  PermissionId = p.PermissionId,
                                  EmployeeFirstName = p.EmployeeFirstName,
                                  EmployeeLastName = p.EmployeeLastName,
                                  PermissionTypeId = p.PermissionType.PermissionTypeId,
                                  PermissionDescription = p.PermissionType != null ? p.PermissionType.PermissionDescription : "No description available",
                                  PermissionDate = p.PermissionDate
                              })
                              .ToListAsync();
                if (results is null || !results.Any()) return NotFound("No permissions found.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving permissions.");
                return StatusCode(500, "An error occurred. Please try again later.");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmployeePermissionById(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid ID.");

            try
            {
                var results = await _dbContext.Permissions
                    .AsNoTracking()
                    .Include(p => p.PermissionType)
                    .FirstOrDefaultAsync(p => p.PermissionId == id);

                if (results is null) return NotFound("No permissions found.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving permissions for ID {PermissionId}.", id);
                return Problem("An error occurred. Please try again later.", statusCode: 500);
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateEmployeePermission(Guid id, UpdateEmployeePermissionDto updatePermission)
        {
            try
            {
                if (updatePermission is null) return BadRequest("Invalid request.");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var employeePermission = await _dbContext.Permissions
                                        .Include(p => p.PermissionType)
                                        .FirstOrDefaultAsync(p => p.PermissionId == id);

                if (employeePermission is null) return NotFound("No permissions found.");

                employeePermission.PermissionDate = DateTime.UtcNow;
                employeePermission.EmployeeFirstName = updatePermission.EmployeeFirstName;
                employeePermission.EmployeeLastName = updatePermission.EmployeeLastName;

                var permissionType = await _dbContext.PermissionTypes
                    .FirstOrDefaultAsync(pt => pt.PermissionDescription.ToLower() == updatePermission.PermissionDescription.ToLower());

                if (permissionType is null)
                {
                    permissionType = new PermissionType
                    {
                        PermissionTypeId = Guid.NewGuid(),
                        PermissionDescription = updatePermission.PermissionDescription
                    };
                    _dbContext.PermissionTypes.Add(permissionType);
                }

                employeePermission.PermissionType = permissionType;
                await _dbContext.SaveChangesAsync();

                PermissionDto permissionDto = new PermissionDto {
                    PermissionId = employeePermission.PermissionId,
                    EmployeeFirstName = employeePermission.EmployeeFirstName,
                    EmployeeLastName = employeePermission.EmployeeLastName,
                    PermissionTypeId = employeePermission.PermissionTypeId,
                    PermissionDescription = employeePermission.PermissionType.PermissionDescription,
                    PermissionDate = employeePermission.PermissionDate
                };

                return Ok(permissionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating permissions for ID {PermissionId}.", id);
                return Problem("An error occurred. Please try again later.", statusCode: 500);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeePermission(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid ID.");

            try
            {
                var permissionToDelete = await _dbContext.Permissions
                    .Include(p => p.PermissionType)
                    .FirstOrDefaultAsync(p => p.PermissionId == id);

                if (permissionToDelete is null) return NotFound("No permissions found.");

                var isPermissionTypeInUse = await _dbContext.Permissions
                    .AnyAsync(p => p.PermissionType.PermissionDescription == permissionToDelete.PermissionType.PermissionDescription
                                   && p.PermissionId != id);

                _dbContext.Permissions.Remove(permissionToDelete);

                if (!isPermissionTypeInUse) _dbContext.PermissionTypes.Remove(permissionToDelete.PermissionType);
                
                await _dbContext.SaveChangesAsync();
                return Ok("Permission and associated type deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the permission with ID {PermissionId}.", id);
                return Problem("An error occurred. Please try again later.", statusCode: 500);
            }
        }
    }
}
