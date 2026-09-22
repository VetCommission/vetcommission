using System;
using System.Collections.Generic;

namespace VetCommission.Infrastructure.Persistence.Generated.Entities;

public partial class Clinic
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string? LegalName { get; set; }

    public string? Document { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? InactivatedAtUtc { get; set; }

    public virtual ICollection<ProcedureCategory> ProcedureCategories { get; set; } = new List<ProcedureCategory>();

    public virtual ICollection<ProfessionalRole> ProfessionalRoles { get; set; } = new List<ProfessionalRole>();

    public virtual ICollection<ProfessionalSpecialty> ProfessionalSpecialties { get; set; } = new List<ProfessionalSpecialty>();

    public virtual ICollection<Professional> Professionals { get; set; } = new List<Professional>();

    public virtual Tenant Tenant { get; set; } = null!;
}
