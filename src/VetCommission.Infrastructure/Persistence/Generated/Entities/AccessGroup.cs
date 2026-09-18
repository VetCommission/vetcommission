using System;
using System.Collections.Generic;

namespace VetCommission.Infrastructure.Persistence.Generated.Entities;

public partial class AccessGroup
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public virtual ICollection<AccessGroupResource> AccessGroupResources { get; set; } = new List<AccessGroupResource>();

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual ICollection<UserTenant> UserTenants { get; set; } = new List<UserTenant>();
}
