using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VetCommission.Infrastructure.Persistence.Generated.Entities;

namespace VetCommission.Infrastructure.Persistence.Generated;

public partial class VetCommissionDbContext : DbContext
{
    public VetCommissionDbContext(DbContextOptions<VetCommissionDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessGroup> AccessGroups { get; set; }

    public virtual DbSet<AccessGroupResource> AccessGroupResources { get; set; }

    public virtual DbSet<AccessResource> AccessResources { get; set; }

    public virtual DbSet<Clinic> Clinics { get; set; }

    public virtual DbSet<DatabaseVersion> DatabaseVersions { get; set; }

    public virtual DbSet<Procedure> Procedures { get; set; }

    public virtual DbSet<ProcedureCategory> ProcedureCategories { get; set; }

    public virtual DbSet<Professional> Professionals { get; set; }

    public virtual DbSet<ProfessionalRole> ProfessionalRoles { get; set; }

    public virtual DbSet<ProfessionalSpecialty> ProfessionalSpecialties { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTenant> UserTenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_access_group");

            entity.ToTable("access_group", "core");

            entity.HasIndex(e => new { e.TenantId, e.Active }, "ix_access_group_tenant_id_active");

            entity.HasIndex(e => new { e.Id, e.TenantId }, "uk_access_group_id_tenant_id").IsUnique();

            entity.HasIndex(e => new { e.TenantId, e.Code }, "uk_access_group_tenant_id_code").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at_utc");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .HasColumnName("name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasOne(d => d.Tenant).WithMany(p => p.AccessGroups)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_access_group_tenant");
        });

        modelBuilder.Entity<AccessGroupResource>(entity =>
        {
            entity.HasKey(e => new { e.AccessGroupId, e.AccessResourceId }).HasName("pk_access_group_resource");

            entity.ToTable("access_group_resource", "core");

            entity.HasIndex(e => e.AccessResourceId, "ix_access_group_resource_access_resource_id");

            entity.Property(e => e.AccessGroupId).HasColumnName("access_group_id");
            entity.Property(e => e.AccessResourceId).HasColumnName("access_resource_id");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at_utc");

            entity.HasOne(d => d.AccessGroup).WithMany(p => p.AccessGroupResources)
                .HasForeignKey(d => d.AccessGroupId)
                .HasConstraintName("fk_access_group_resource_access_group");

            entity.HasOne(d => d.AccessResource).WithMany(p => p.AccessGroupResources)
                .HasForeignKey(d => d.AccessResourceId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_access_group_resource_access_resource");
        });

        modelBuilder.Entity<AccessResource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_access_resource");

            entity.ToTable("access_resource", "core");

            entity.HasIndex(e => e.ResourceKey, "uk_access_resource_resource_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at_utc");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(160)
                .HasColumnName("name");
            entity.Property(e => e.ResourceKey)
                .HasMaxLength(160)
                .HasColumnName("resource_key");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");
        });

        modelBuilder.Entity<Clinic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_clinic");

            entity.ToTable("clinic", "business");

            entity.HasIndex(e => new { e.TenantId, e.Active }, "ix_clinic_tenant_active");

            entity.HasIndex(e => new { e.TenantId, e.Name }, "ix_clinic_tenant_name");

            entity.HasIndex(e => new { e.TenantId, e.Document }, "ux_clinic_tenant_document")
                .IsUnique()
                .HasFilter("(document IS NOT NULL)");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at_utc");
            entity.Property(e => e.Document)
                .HasMaxLength(30)
                .HasColumnName("document");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .HasColumnName("email");
            entity.Property(e => e.InactivatedAtUtc).HasColumnName("inactivated_at_utc");
            entity.Property(e => e.LegalName)
                .HasMaxLength(200)
                .HasColumnName("legal_name");
            entity.Property(e => e.Name)
                .HasMaxLength(160)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(40)
                .HasColumnName("phone");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Clinics)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_clinic_tenant");
        });

        modelBuilder.Entity<DatabaseVersion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_database_version");

            entity.ToTable("database_version", "core");

            entity.HasIndex(e => e.ScriptName, "uk_database_version_script_name").IsUnique();

            entity.HasIndex(e => e.Version, "uk_database_version_version").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AppliedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("applied_at_utc");
            entity.Property(e => e.AppliedBy)
                .HasMaxLength(100)
                .HasDefaultValueSql("CURRENT_USER")
                .HasColumnName("applied_by");
            entity.Property(e => e.ChecksumSha256)
                .HasMaxLength(64)
                .IsFixedLength()
                .HasColumnName("checksum_sha256");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.ScriptName)
                .HasMaxLength(255)
                .HasColumnName("script_name");
            entity.Property(e => e.Version)
                .HasMaxLength(20)
                .HasColumnName("version");
        });

        modelBuilder.Entity<Procedure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_procedure");

            entity.ToTable("procedure", "business");

            entity.HasIndex(e => new { e.TenantId, e.ClinicId, e.CategoryId, e.Active }, "ix_procedure_tenant_clinic_category_active");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.Code)
                .HasMaxLength(60)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at_utc");
            entity.Property(e => e.DefaultValue)
                .HasPrecision(12, 2)
                .HasColumnName("default_value");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.InactivatedAtUtc).HasColumnName("inactivated_at_utc");
            entity.Property(e => e.Name)
                .HasMaxLength(160)
                .HasColumnName("name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasOne(d => d.Category).WithMany(p => p.Procedures)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_procedure_category");

            entity.HasOne(d => d.Clinic).WithMany(p => p.Procedures)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_procedure_clinic");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Procedures)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_procedure_tenant");
        });

        modelBuilder.Entity<ProcedureCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_procedure_category");

            entity.ToTable("procedure_category", "business");

            entity.HasIndex(e => new { e.TenantId, e.ClinicId, e.Active }, "ix_procedure_category_tenant_clinic_active");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at_utc");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.InactivatedAtUtc).HasColumnName("inactivated_at_utc");
            entity.Property(e => e.Name)
                .HasMaxLength(160)
                .HasColumnName("name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasOne(d => d.Clinic).WithMany(p => p.ProcedureCategories)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_procedure_category_clinic");

            entity.HasOne(d => d.Tenant).WithMany(p => p.ProcedureCategories)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_procedure_category_tenant");
        });

        modelBuilder.Entity<Professional>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_professional");

            entity.ToTable("professional", "business");

            entity.HasIndex(e => new { e.TenantId, e.ClinicId, e.Active }, "ix_professional_tenant_clinic_active");

            entity.HasIndex(e => new { e.TenantId, e.Active }, "ix_professional_tenant_id_active");

            entity.HasIndex(e => new { e.TenantId, e.Name }, "ix_professional_tenant_id_name");

            entity.HasIndex(e => new { e.TenantId, e.UserId }, "ux_professional_tenant_id_user_id")
                .IsUnique()
                .HasFilter("(user_id IS NOT NULL)");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at_utc");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .HasColumnName("email");
            entity.Property(e => e.InactivatedAtUtc).HasColumnName("inactivated_at_utc");
            entity.Property(e => e.Name)
                .HasMaxLength(160)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(40)
                .HasColumnName("phone");
            entity.Property(e => e.ProfessionalRegistration)
                .HasMaxLength(80)
                .HasColumnName("professional_registration");
            entity.Property(e => e.Role)
                .HasMaxLength(120)
                .HasColumnName("role");
            entity.Property(e => e.Specialty)
                .HasMaxLength(120)
                .HasColumnName("specialty");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Clinic).WithMany(p => p.Professionals)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_professional_clinic");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Professionals)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_professional_tenant");

            entity.HasOne(d => d.User).WithMany(p => p.Professionals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_professional_user");
        });

        modelBuilder.Entity<ProfessionalRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_professional_role");

            entity.ToTable("professional_role", "business");

            entity.HasIndex(e => new { e.TenantId, e.Active }, "ix_professional_role_tenant_active");

            entity.HasIndex(e => new { e.TenantId, e.ClinicId, e.Active }, "ix_professional_role_tenant_clinic_active");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at_utc");
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .HasColumnName("name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasOne(d => d.Clinic).WithMany(p => p.ProfessionalRoles)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_professional_role_clinic");

            entity.HasOne(d => d.Tenant).WithMany(p => p.ProfessionalRoles)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_professional_role_tenant");
        });

        modelBuilder.Entity<ProfessionalSpecialty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_professional_specialty");

            entity.ToTable("professional_specialty", "business");

            entity.HasIndex(e => new { e.TenantId, e.Active }, "ix_professional_specialty_tenant_active");

            entity.HasIndex(e => new { e.TenantId, e.ClinicId, e.Active }, "ix_professional_specialty_tenant_clinic_active");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at_utc");
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .HasColumnName("name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasOne(d => d.Clinic).WithMany(p => p.ProfessionalSpecialties)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_professional_specialty_clinic");

            entity.HasOne(d => d.Tenant).WithMany(p => p.ProfessionalSpecialties)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_professional_specialty_tenant");
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tenant");

            entity.ToTable("tenant", "core");

            entity.HasIndex(e => e.Slug, "uk_tenant_slug").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at_utc");
            entity.Property(e => e.Name)
                .HasMaxLength(160)
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasMaxLength(80)
                .HasColumnName("slug");
            entity.Property(e => e.Timezone)
                .HasMaxLength(80)
                .HasDefaultValueSql("'America/Sao_Paulo'::character varying")
                .HasColumnName("timezone");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_user");

            entity.ToTable("user", "core");

            entity.HasIndex(e => e.NormalizedEmail, "uk_user_normalized_email").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at_utc");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .HasColumnName("email");
            entity.Property(e => e.LastLoginAtUtc).HasColumnName("last_login_at_utc");
            entity.Property(e => e.Name)
                .HasMaxLength(160)
                .HasColumnName("name");
            entity.Property(e => e.NormalizedEmail)
                .HasMaxLength(254)
                .HasColumnName("normalized_email");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.PasswordUpdatedAtUtc).HasColumnName("password_updated_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");
        });

        modelBuilder.Entity<UserTenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_user_tenant");

            entity.ToTable("user_tenant", "core");

            entity.HasIndex(e => new { e.TenantId, e.Active }, "ix_user_tenant_tenant_id_active");

            entity.HasIndex(e => new { e.UserId, e.Active }, "ix_user_tenant_user_id_active");

            entity.HasIndex(e => new { e.UserId, e.TenantId }, "uk_user_tenant_user_id_tenant_id").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccessGroupId).HasColumnName("access_group_id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at_utc");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Tenant).WithMany(p => p.UserTenants)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_user_tenant_tenant");

            entity.HasOne(d => d.User).WithMany(p => p.UserTenants)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_user_tenant_user");

            entity.HasOne(d => d.AccessGroup).WithMany(p => p.UserTenants)
                .HasPrincipalKey(p => new { p.Id, p.TenantId })
                .HasForeignKey(d => new { d.AccessGroupId, d.TenantId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_user_tenant_access_group_same_tenant");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
