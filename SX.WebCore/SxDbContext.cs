using Microsoft.AspNet.Identity.EntityFramework;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore
{
    public class SxDbContext : IdentityDbContext<SxAppUser>
    {
        //Строка поделючения задается здесь
        private static string _nameOrConnectionString;
        public SxDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString) 
        {
            _nameOrConnectionString = nameOrConnectionString;
        }
        public static TDbContext Create<TDbContext>()
        {
            var context = Activator.CreateInstance<TDbContext>();
            return context;
        }

        public DbSet<SxArticle> Articles { get; set; }

        public DbSet<SxClick> Clicks { get; set; }

        public DbSet<SxMenu> Menues { get; set; }

        public DbSet<SxNews> News { get; set; }

        public DbSet<SxPicture> Pictures { get; set; }

        public DbSet<SxRedirect> Redirects { get; set; }

        public DbSet<SxRequest> Requestes { get; set; }

        public DbSet<SxSeoInfo> SeoInfo { get; set; }

        public DbSet<SxSeoKeyword> SeoKeyWords { get; set; }

        public DbSet<SxSiteSetting> SiteSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SxMaterial>()
                .HasKey(x => new { x.Id, x.ModelCoreType });

            modelBuilder.Entity<SxMaterialTag>()
               .HasOptional(x => x.Material)
               .WithMany()
               .HasForeignKey(x => new { x.MaterialId, x.ModelCoreType });

            modelBuilder.Entity<SxSiteSetting>()
                .Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
