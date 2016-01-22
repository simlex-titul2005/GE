using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext() : base("DbContext") { }

        public DbSet<SeoKeyWord> SeoKeyWords { get; set; }

        public DbSet<SeoInfo> SeoInfo { get; set; }

        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Material>()
                .HasKey(x => new { x.Id, x.CoreType });

            modelBuilder.Entity<SeoInfo>()
                .HasRequired(x => x.Material)
                .WithMany()
                .HasForeignKey(x => new { x.MeterialId, x.MaterialCoreType });
        }
    }
}
