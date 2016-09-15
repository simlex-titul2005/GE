using GE.WebUI.Models;
using SX.WebCore;
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

        public new DbSet<News> News { get; set; }

        public DbSet<SiteTest> SiteTests { get; set; }

        public DbSet<SiteTestQuestion> SiteTestQuestions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Aphorism>().HasOptional(x => x.Author).WithMany(x => x.Aphorisms).HasForeignKey(x => new { x.AuthorId }).WillCascadeOnDelete(false);

            modelBuilder.Entity<SiteTestAnswer>().HasKey(x => new { x.QuestionId, x.SubjectId });
            modelBuilder.Entity<SiteTestAnswer>().HasRequired(x => x.Question).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SiteTestAnswer>().HasRequired(x => x.Subject).WithMany().WillCascadeOnDelete(false);
        }
    }
}