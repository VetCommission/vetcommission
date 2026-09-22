using System;
using System.Collections.Generic;

namespace VetCommission.Infrastructure.Persistence.Generated.Entities;

public partial class ProfessionalRole
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public Guid ClinicId { get; set; }

    public virtual Clinic Clinic { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;
}
