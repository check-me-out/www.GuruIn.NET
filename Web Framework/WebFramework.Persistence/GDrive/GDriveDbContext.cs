using System.Configuration;
using System.Data.Entity;
using WebFramework.Persistence.Helpers;
using WebFramework.Models.GDrive;

namespace WebFramework.Persistence.GDrive
{
    public class GDriveDbContext : DbContext, IGDriveDbContext
    {
        public GDriveDbContext()
            : base("name=BlogDbConnection")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<FileContent> Files { get; set; }
    }
}
