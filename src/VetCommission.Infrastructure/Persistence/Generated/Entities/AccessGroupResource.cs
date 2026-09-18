using System;
using System.Collections.Generic;

namespace VetCommission.Infrastructure.Persistence.Generated.Entities;

public partial class AccessGroupResource
{
    public Guid AccessGroupId { get; set; }

    public Guid AccessResourceId { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public virtual AccessGroup AccessGroup { get; set; } = null!;

    public virtual AccessResource AccessResource { get; set; } = null!;
}
