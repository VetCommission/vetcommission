using System;
using System.Collections.Generic;

namespace VetCommission.Infrastructure.Persistence.Generated.Entities;

public partial class Competency
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid ClinicId { get; set; }

    public short Year { get; set; }

    public short Month { get; set; }

    public string Status { get; set; } = null!;

    public DateTime OpenedAtUtc { get; set; }

    public DateTime? ClosedAtUtc { get; set; }

    public virtual Clinic Clinic { get; set; } = null!;

    public virtual ICollection<CommissionRule> CommissionRules { get; set; } = new List<CommissionRule>();

    public virtual Tenant Tenant { get; set; } = null!;
}
