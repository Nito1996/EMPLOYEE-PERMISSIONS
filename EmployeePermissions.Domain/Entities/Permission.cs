using System;
using System.Text.Json.Serialization;

namespace EmployeePermissions.Domain.Entities
{
    public class Permission
    {
        public Guid PermissionId { get; set; } = Guid.NewGuid();
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public Guid PermissionTypeId { get; set; }
        public PermissionType PermissionType { get; set; }
        [JsonIgnore]
        public DateTimeOffset PermissionDate { get; set; }
    }
}
