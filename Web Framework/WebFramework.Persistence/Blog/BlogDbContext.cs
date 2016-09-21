using WebFramework.Models.Blog;
using System.Configuration;
using System.Data.Entity;
using WebFramework.Persistence.Helpers;

namespace WebFramework.Persistence.Blog
{
    public class BlogDbContext : DbContext, IBlogDbContext
    {
        public BlogDbContext()
            : base("name=BlogDbConnection")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<BadWords> BadWords { get; set; }
    }
}
