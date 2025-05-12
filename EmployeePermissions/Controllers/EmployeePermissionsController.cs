using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeePermissions.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeePermissions.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger<EmployeePermissionsController> _logger;

        public EmployeePermissionsController(IPermissionService permissionService, ILogger<EmployeePermissionsController> logger)
        {
            _permissionService = permissionService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeePermission([FromBody] CreateEmployeePermissionDto newPermission)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var permissionDto = await _permissionService.CreateAsync(newPermission);
                return StatusCode(201, permissionDto);
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
                var results = await _permissionService.GetAllAsync();
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
                var result = await _permissionService.GetByIdAsync(id);
                if (result is null) return NotFound("No permissions found.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving permissions for ID {PermissionId}.", id);
                return Problem("An error occurred. Please try again later.", statusCode: 500);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployeePermission(Guid id, UpdateEmployeePermissionDto updatePermission)
        {
            try
            {
                if (updatePermission is null) return BadRequest("Invalid request.");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                await _permissionService.UpdateAsync(id, updatePermission);
                return Ok("Permission updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating permissions for ID {PermissionId}.", id);
                return Problem("An error occurred. Please try again later.", statusCode: 500);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeePermission(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid ID.");

            try
            {
                await _permissionService.DeleteAsync(id);
                return Ok("Permission deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Permission with ID {PermissionId} not found.", id);
                return NotFound("Permission not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the permission with ID {PermissionId}.", id);
                return Problem("An error occurred. Please try again later.", statusCode: 500);
            }
        }
    }
}