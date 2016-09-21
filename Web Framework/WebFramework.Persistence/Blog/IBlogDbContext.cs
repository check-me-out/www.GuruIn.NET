using WebFramework.Models.Blog;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace WebFramework.Persistence.Blog
{
    public interface IBlogDbContext : IDisposable
    {
        int SaveChanges();

        DbEntityEntry Entry(object entity);

        DbSet<Post> Posts { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<BadWords> BadWords { get; set; }
    }
}
