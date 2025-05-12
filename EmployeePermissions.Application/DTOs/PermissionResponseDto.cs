using System;

public class PermissionResponseDto
{
    public Guid PermissionId { get; set; }
    public string EmployeeFirstName { get; set; }
    public string EmployeeLastName { get; set; }
    public Guid PermissionTypeId { get; set; }
    public DateTimeOffset PermissionDate { get; set; }
    public string PermissionDescription { get; set; }
}