using WebFramework.Models.Products;
using System.Configuration;
using System.Data.Entity;
using WebFramework.Persistence.Helpers;

namespace WebFramework.Persistence.Products
{
    public class ProductsDbContext : DbContext, IProductsDbContext
    {
        public ProductsDbContext()
            : base("name=ProductsDbConnection")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<SizeUnit> SizeUnits { get; set; }
        public virtual DbSet<Dimension> Dimensions { get; set; }
        public virtual DbSet<Item> Items { get; set; }
    }
}
