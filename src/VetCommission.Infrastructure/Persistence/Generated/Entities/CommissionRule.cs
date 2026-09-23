using System;
using System.Collections.Generic;

namespace VetCommission.Infrastructure.Persistence.Generated.Entities;

public partial class CommissionRule
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid ClinicId { get; set; }

    public Guid CompetencyId { get; set; }

    public Guid ProcedureId { get; set; }

    public string RuleType { get; set; } = null!;

    public decimal? Percentage { get; set; }

    public decimal? FixedValue { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public virtual Clinic Clinic { get; set; } = null!;

    public virtual Competency Competency { get; set; } = null!;

    public virtual Procedure Procedure { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;
}
