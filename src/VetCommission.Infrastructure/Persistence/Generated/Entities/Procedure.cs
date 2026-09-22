using System;
using System.Collections.Generic;

namespace VetCommission.Infrastructure.Persistence.Generated.Entities;

public partial class Procedure
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid ClinicId { get; set; }

    public Guid CategoryId { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal DefaultValue { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? InactivatedAtUtc { get; set; }

    public virtual ProcedureCategory Category { get; set; } = null!;

    public virtual Clinic Clinic { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;
}
