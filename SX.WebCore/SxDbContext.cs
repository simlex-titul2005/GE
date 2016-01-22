using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore
{
    public class SxDbContext : System.Data.Entity.DbContext
    {
        //Строка поделючения задается здесь
        public SxDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        public DbSet<SxArticle> Articles { get; set; }

        public DbSet<SxNews> News { get; set; }

        public DbSet<SxPicture> Pictures { get; set; }

        public DbSet<SxSeoInfo> SeoInfo { get; set; }

        public DbSet<SxSeoKeyWord> SeoKeyWords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SxMaterial>()
                .HasKey(x => new { x.Id, x.CoreType });

            modelBuilder.Entity<SxMaterialTag>()
               .HasOptional(x => x.Material)
               .WithMany()
               .HasForeignKey(x => new { x.MaterialId, x.MaterialCoreType });

            modelBuilder.Entity<SxPicture>()
                            .HasKey(x => new { x.Id, x.MaterialId, x.Width })
                            .HasRequired(x => x.Material)
                            .WithMany()
                            .HasForeignKey(x => new { x.MaterialId, x.MaterialCoreType });

            modelBuilder.Entity<SxSeoInfo>()
                .HasRequired(x => x.Material)
                .WithMany()
                .HasForeignKey(x => new { x.Material, x.MaterialCoreType });
        }
    }
}
