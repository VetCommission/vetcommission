using System;
using System.Collections.Generic;

namespace VetCommission.Infrastructure.Persistence.Generated.Entities;

public partial class Tenant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Timezone { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public virtual ICollection<AccessGroup> AccessGroups { get; set; } = new List<AccessGroup>();

    public virtual ICollection<Clinic> Clinics { get; set; } = new List<Clinic>();

    public virtual ICollection<ProcedureCategory> ProcedureCategories { get; set; } = new List<ProcedureCategory>();

    public virtual ICollection<ProfessionalRole> ProfessionalRoles { get; set; } = new List<ProfessionalRole>();

    public virtual ICollection<ProfessionalSpecialty> ProfessionalSpecialties { get; set; } = new List<ProfessionalSpecialty>();

    public virtual ICollection<Professional> Professionals { get; set; } = new List<Professional>();

    public virtual ICollection<UserTenant> UserTenants { get; set; } = new List<UserTenant>();
}
