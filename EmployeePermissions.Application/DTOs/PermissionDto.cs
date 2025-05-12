using System;

public class PermissionDto
{
    public Guid PermissionId { get; set; } = Guid.NewGuid();
    public string EmployeeFirstName { get; set; }
    public string EmployeeLastName { get; set; }
    public Guid PermissionTypeId { get; set; } = Guid.NewGuid();
    public string PermissionDescription { get; set; }
    public DateTimeOffset PermissionDate { get; set; }
}