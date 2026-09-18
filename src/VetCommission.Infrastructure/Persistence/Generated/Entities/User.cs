using System;
using System.Collections.Generic;

namespace VetCommission.Infrastructure.Persistence.Generated.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string NormalizedEmail { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? PasswordUpdatedAtUtc { get; set; }

    public DateTime? LastLoginAtUtc { get; set; }

    public virtual ICollection<UserTenant> UserTenants { get; set; } = new List<UserTenant>();
}
