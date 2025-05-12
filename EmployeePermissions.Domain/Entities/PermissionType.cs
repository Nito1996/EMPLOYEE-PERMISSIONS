using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EmployeePermissions.Domain.Entities
{
    public class PermissionType
    {
        [JsonIgnore]
        public Guid PermissionTypeId { get; set; } = Guid.NewGuid();
        public string PermissionDescription { get; set; }
        [JsonIgnore]
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}