using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Openlatino.Admin.Infrastucture.Services;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Internationalization;
using System.Security;
using OpenLatino.Admin.Infrastructure.DataContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Openlatino.Admin.Infrastucture.DataContexts
{
    public class AdminDb : UnitOfWork
    {
        public AdminDb(DbContextOptions<AdminDb> options) : base(options)
        { }
        public DbSet<TematicType> TematicTypes { get; set; }
        public DbSet<TematicTypesClasification> TematicTypesClasifications { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceFunction> ServiceFunctions { get; set; }
        public DbSet<WorkspaceFunction> WorkspaceFunctions { get; set; }
        public DbSet<WorkSpace> WorkSpaces { get; set; }

        public DbSet<StyleConfig> StyleConfigs { get; set; }
        public DbSet<ProviderInfo> Providers { get; set; }
        public DbSet<ProviderTranslations> ProviderTranslations { get; set; }
        public DbSet<Layer> Layers { get; set; }
        public DbSet<LayerTranslation> LayerTranslations { get; set; }
        public DbSet<AlfaInfo> AlfaInfos { get; set; }
        public DbSet<AlfaInfoTranslation> AlfaInfoTranslations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<VectorStyle> Styles { get; set; }
        public DbSet<LayerStyle> LayerStyles { get; set; }
        public DbSet<TematicLayer> TematicLayers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(conf =>
            {
                conf.Ignore(CoreEventId.DetachedLazyLoadingWarning);
                conf.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning);
            });
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region Configure tables 
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<AlfaInfo>(entity =>
            {
                entity.HasIndex(e => e.LayerId)
                    .HasName("IX_LayerID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Columns).IsRequired();

                entity.Property(e => e.ConnectionString).IsRequired();

                entity.Property(e => e.LayerId).HasColumnName("LayerID");

                entity.Property(e => e.PkField).IsRequired();

                entity.Property(e => e.Table).IsRequired();

                entity.HasOne(d => d.Layer)
                    .WithMany(p => p.AlfaInfoes)
                    .HasForeignKey(d => d.LayerId)
                    .HasConstraintName("FK_dbo.AlfaInfoes_dbo.Layers_LayerID");
            });

            modelBuilder.Entity<WorkspaceFunction>(entity =>
            {
                entity.HasKey(e => new { e.WorkSpaceId, e.FunctionId })
                .HasName("PK_dbo.WorkspaceFunction");
            });

            modelBuilder.Entity<AlfaInfoTranslation>(entity =>
            {
                entity.HasKey(e => new { e.LanguageId, e.EntityId })
                    .HasName("PK_dbo.AlfaInfoTranslations");

                entity.HasIndex(e => e.EntityId)
                    .HasName("IX_EntityID");

                entity.HasIndex(e => e.LanguageId)
                    .HasName("IX_LanguageID");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.EntityId).HasColumnName("EntityID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name).IsRequired();



                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AlfaInfoTranslations)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_dbo.AlfaInfoTranslations_dbo.AlfaInfoes_EntityID");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.AlfaInfoTranslations)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK_dbo.AlfaInfoTranslations_dbo.Languages_LanguageID");
            });

            modelBuilder.Entity<LayerStyle>(entity =>
            {
                entity.HasKey(e => new { e.LayerId, e.VectorStyleId })
                .HasName("PK_dbo.LayerStyles");
            });

            modelBuilder.Entity<TematicType>(entity =>
            {
                entity.Property(e => e.Function).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.ID)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<LayerTranslation>(entity =>
            {
                entity.HasKey(e => new { e.LanguageId, e.EntityId })
                    .HasName("PK_dbo.LayerTranslations");

                entity.HasIndex(e => e.EntityId)
                    .HasName("IX_EntityID");

                entity.HasIndex(e => e.LanguageId)
                    .HasName("IX_LanguageID");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.EntityId).HasColumnName("EntityID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.LayerTranslations)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_dbo.LayerTranslations_dbo.Layers_EntityID");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.LayerTranslations)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK_dbo.LayerTranslations_dbo.Languages_LanguageID");
            });

            modelBuilder.Entity<Layer>(entity =>
            {
                entity.HasIndex(e => e.ProviderInfoId)
                    .HasName("IX_ProviderInfoId");

                entity.HasOne(d => d.ProviderInfo)
                    .WithMany(p => p.Layers)
                    .HasForeignKey(d => d.ProviderInfoId)
                    .HasConstraintName("FK_dbo.Layers_dbo.ProviderInfoes_ProviderInfoId");
            });

            modelBuilder.Entity<TematicLayer>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey })
                    .HasName("PK_dbo.__MigrationHistory");

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.Model).IsRequired();

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<ProviderInfo>(entity =>
            {
                entity.Property(e => e.BoundingBoxField).IsRequired();

                entity.Property(e => e.ConnectionString).IsRequired();

                entity.Property(e => e.GeoField).IsRequired();

                entity.Property(e => e.PkField).IsRequired();

                entity.Property(e => e.Table).IsRequired();

                entity.Property(e => e.Type).IsRequired();
            });

            modelBuilder.Entity<ProviderTranslations>(entity =>
            {
                entity.HasKey(e => new { e.LanguageId, e.EntityId })
                    .HasName("PK_dbo.ProviderTranslations");

                entity.HasIndex(e => e.EntityId)
                    .HasName("IX_EntityID");

                entity.HasIndex(e => e.LanguageId)
                    .HasName("IX_LanguageID");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.EntityId).HasColumnName("EntityID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.ProviderTranslations)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_dbo.ProviderTranslations_dbo.ProviderInfoes_EntityID");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.ProviderTranslations)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK_dbo.ProviderTranslations_dbo.Languages_LanguageID");
            });

            modelBuilder.Entity<ServiceFunction>(entity =>
            {
                entity.HasIndex(e => e.ServiceId)
                    .HasName("IX_ServiceId");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceFunctions)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.ServiceFunctions_dbo.Services_ServiceId");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<StyleConfig>(entity =>
            {
                entity.HasKey(e => new { e.LayerId, e.TematicTypeId, e.StyleId, e.TematicLayerId })
                    .HasName("PK_dbo.StyleConfiguration");

                entity.HasIndex(e => e.TematicTypeId)
                    .HasName("IX_TematicTypeId");

                entity.HasIndex(e => e.LayerId)
                    .HasName("IX_LayerId");

                entity.HasIndex(e => e.StyleId)
                    .HasName("IX_StyleId");

                entity.HasIndex(e => e.TematicLayerId)
                    .HasName("IX_TematicLayerId");

                entity.HasOne(d => d.TematicType)
                    .WithMany(p => p.StyleConfiguration)
                    .HasForeignKey(d => d.TematicTypeId)
                    .HasConstraintName("FK_dbo.StyleConfiguration_dbo.TematicType_TematicTypeId");

                entity.HasOne(d => d.Layer)
                    .WithMany(p => p.StyleConfiguration)
                    .HasForeignKey(d => d.LayerId)
                    .HasConstraintName("FK_dbo.StyleConfiguration_dbo.Layers_LayerId");

                entity.HasOne(d => d.TematicLayer)
                    .WithMany(p => p.StyleConfiguration)
                    .HasForeignKey(d => d.TematicLayerId)
                    .HasConstraintName("FK_dbo.StyleConfiguration_dbo.TematicLayer_TematicLayerId");

                entity.HasOne(d => d.Style)
                    .WithMany(p => p.StyleConfiguration)
                    .HasForeignKey(d => d.StyleId)
                    .HasConstraintName("FK_dbo.StyleConfiguration_dbo.VectorStyles_StyleId");
            });

            modelBuilder.Entity<ClientWorkSpaces>(entity =>
            {
                entity.HasKey(e => new { e.ClientId, e.WorkSpaceId })
                    .HasName("PK_dbo.ClientWorkSpaces");

                entity.HasIndex(e => e.ClientId)
                    .HasName("IX_Client_Id");

                entity.HasIndex(e => e.WorkSpaceId)
                    .HasName("IX_WorkSpace_Id");

                entity.Property(e => e.ClientId).HasColumnName("Client_Id");

                entity.Property(e => e.WorkSpaceId).HasColumnName("WorkSpace_Id");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientWorkSpaces)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_dbo.ClientWorkSpaces_dbo.Client_Client_Id");

                entity.HasOne(d => d.WorkSpace)
                    .WithMany(p => p.CLientWorkSpaces)
                    .HasForeignKey(d => d.WorkSpaceId)
                    .HasConstraintName("FK_dbo.ClientWorkSpaces_dbo.WorkSpaces_WorkSpace_Id");
            });

            modelBuilder.Entity<LayerWorkspaces>(entity =>
            {
                entity.HasKey(e => new { e.LayerId, e.WorkSpaceId })
                    .HasName("PK_dbo.LayerWorkSpaces");

                entity.HasIndex(e => e.LayerId)
                    .HasName("IX_Layer_Id");

                entity.HasIndex(e => e.WorkSpaceId)
                    .HasName("IX_WorkSpace_Id");

                entity.Property(e => e.LayerId).HasColumnName("Layer_Id");

                entity.Property(e => e.WorkSpaceId).HasColumnName("WorkSpace_Id");

                entity.HasOne(d => d.Layer)
                    .WithMany(p => p.LayerWorkspaces)
                    .HasForeignKey(d => d.LayerId)
                    .HasConstraintName("FK_dbo.LayerWorkSpaces_dbo.Layer_Layer_Id");

                entity.HasOne(d => d.Workspace)
                    .WithMany(p => p.LayerWorkspaces)
                    .HasForeignKey(d => d.WorkSpaceId)
                    .HasConstraintName("FK_dbo.LayerWorkSpaces_dbo.WorkSpaces_WorkSpace_Id");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AllowedOrigin).HasMaxLength(100);
            });

            modelBuilder.Entity<VectorStyle>(entity =>
            {
                //entity.HasIndex(e => e.LayerId)
                //    .HasName("IX_Layer_Id");

                entity.Property(e => e.Fill).IsRequired();

                //entity.Property(e => e.LayerId).HasColumnName("Layer_Id");

                entity.Property(e => e.Line).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.OutlinePen).IsRequired();

                entity.Property(e => e.PointFill).IsRequired();

                //entity.HasOne(d => d.Layer)
                //    .WithMany(p => p.VectorStyles)
                //    .HasForeignKey(d => d.LayerId)
                //    .HasConstraintName("FK_dbo.VectorStyles_dbo.Layers_Layer_Id");
            });

            modelBuilder.Entity<WorkSpace>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });
            #endregion
            modelBuilder.Seed();
        }
    }
}