using WebFramework.Models.Products;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace WebFramework.Persistence.Products
{
    public interface IProductsDbContext : IDisposable
    {
        int SaveChanges();

        DbEntityEntry Entry(object entity);

        DbSet<Category> Categories { get; set; }
        DbSet<SubCategory> SubCategories { get; set; }
        DbSet<Color> Colors { get; set; }
        DbSet<Size> Sizes { get; set; }
        DbSet<SizeUnit> SizeUnits { get; set; }
        DbSet<Dimension> Dimensions { get; set; }
        DbSet<Item> Items { get; set; }
    }
}
