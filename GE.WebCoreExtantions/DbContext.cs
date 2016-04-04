using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace GE.WebCoreExtantions
{
    public class DbContext : SX.WebCore.SxDbContext
    {
        public DbContext() : base("DbContext") { }

        public new DbSet<Article> Articles { get; set; }

        public DbSet<Contest> Contests { get; set; }

        public DbSet<Game> Games { get; set; }

        public new DbSet<News> News { get; set; }

        public DbSet<NewsRubric> NewsRubrics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ArticleType>()
                .HasKey(x => new { x.Name, x.GameId });

            modelBuilder.Entity<Article>()
                .HasOptional(x => x.ArticleType)
                .WithMany()
                .HasForeignKey(x => new { x.ArticleTypeName, x.ArticleTypeGameId });

            modelBuilder.Entity<ContestPrize>()
                .HasRequired(x => x.Contest)
                .WithMany(x=>x.Prizes)
                .HasForeignKey(x => new { x.ContestId, x.MaterialCoreType });

            modelBuilder.Entity<NewsRubric>()
                .Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
