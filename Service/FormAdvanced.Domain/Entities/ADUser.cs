using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Domain.Entities
{
    public sealed record ADUser(
        string? Id,
        List<AppRoleAssignment>? AppRoleAssignments,
        AuthorizationInfo? AuthorizationInfo,
        string? Country,
        string? Department,
        List<DirectoryObject>? DirectReports,
        string? DisplayName,
        string? EmployeeId,
        EmployeeOrgData? EmployeeOrgData,
        List<ObjectIdentity>? Identities,
        string? JobTitle,
        string? Mail,
        DirectoryObject? Manager,
        List<DirectoryObject>? MemberOf,
        string? MobilePhone,
        string? UserPrincipalName
        );
}
