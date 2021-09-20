using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EntityConfig.Models
{
    public partial class ConfigDBContext : DbContext
    {
        public ConfigDBContext()
        {
        }

        public ConfigDBContext(DbContextOptions<ConfigDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<VipConfig> VipConfigs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            modelBuilder.Entity<VipConfig>(entity =>
            {
                entity.ToTable("vip_config");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.RequireVip)
                    .HasColumnType("int(11)")
                    .HasColumnName("require_vip");

                entity.Property(e => e.VipName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("vip_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
