using System;
using System.Collections.Generic;

namespace VetCommission.Infrastructure.Persistence.Generated.Entities;

public partial class Professional
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid? UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string Role { get; set; } = null!;

    public string? ProfessionalRegistration { get; set; }

    public string? Specialty { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? InactivatedAtUtc { get; set; }

    public Guid ClinicId { get; set; }

    public virtual Clinic Clinic { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual User? User { get; set; }
}
