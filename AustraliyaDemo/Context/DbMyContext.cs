using AustraliyaDemo.Entity;
using Microsoft.EntityFrameworkCore;

namespace AustraliyaDemo.Context
{
    public class DbMyContext:DbContext
    {
        internal IEnumerable<object> result;

        public DbMyContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleType> ArticleTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ReportComment> ReportComments { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SiteType> SiteTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
    }
}
