using System;
using System.Collections.Generic;

namespace VetCommission.Infrastructure.Persistence.Generated.Entities;

public partial class ProcedureCategory
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid ClinicId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? InactivatedAtUtc { get; set; }

    public virtual Clinic Clinic { get; set; } = null!;

    public virtual ICollection<Procedure> Procedures { get; set; } = new List<Procedure>();

    public virtual Tenant Tenant { get; set; } = null!;
}
