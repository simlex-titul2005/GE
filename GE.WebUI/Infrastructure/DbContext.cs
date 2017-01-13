using GE.WebUI.Models;
using SX.WebCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace GE.WebUI.Infrastructure
{
    public sealed class DbContext : SxDbContext
    {
        public DbContext() : base("DbContext") { }

        public new DbSet<Article> Articles { get; set; }

        public DbSet<Aphorism> Aphorisms { get; set; }

        public DbSet<AuthorAphorism> AuthorAphorisms { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Humor> Humors { get; set; }

        public DbSet<Infographic> Infographics { get; set; }

        public new DbSet<News> News { get; set; }

        public DbSet<PopularYoutubeVideo> PopularYoutubeVideos { get; set; }

        public DbSet<SiteTest> SiteTests { get; set; }

        public DbSet<SiteTestQuestion> SiteTestQuestions { get; set; }

        public DbSet<SteamApp> SteamApps { get; set; }

        public DbSet<SteamNews> SteamNews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Aphorism>().HasOptional(x => x.Author).WithMany(x => x.Aphorisms).HasForeignKey(x => new { x.AuthorId }).WillCascadeOnDelete(false);

            modelBuilder.Entity<Infographic>().HasRequired(x => x.Picture).WithMany().HasForeignKey(x => x.PictureId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Infographic>().HasRequired(x => x.Material).WithMany().HasForeignKey(x => new { x.MaterialId, x.ModelCoreType }).WillCascadeOnDelete(true);

            modelBuilder.Entity<PopularYoutubeVideo>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<SiteTestAnswer>().HasKey(x => new { x.QuestionId, x.SubjectId });
            modelBuilder.Entity<SiteTestAnswer>().HasRequired(x => x.Question).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SiteTestAnswer>().HasRequired(x => x.Subject).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<SiteTestSetting>().HasKey(x=>x.TestId).HasRequired(x => x.Test).WithOptional(x=>x.Settings).WillCascadeOnDelete(true);

            modelBuilder.Entity<SteamApp>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<SteamNews>().Property(x => x.Gid).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<SteamNews>().HasRequired(x => x.TheNews).WithMany().HasForeignKey(x => new { x.TheNewsId, x.ModelCoreType }).WillCascadeOnDelete(true);
        }
    }
}