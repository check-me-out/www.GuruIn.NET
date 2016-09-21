using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WebFramework.Models.GDrive;

namespace WebFramework.Persistence.GDrive
{
    public interface IGDriveDbContext : IDisposable
    {
        int SaveChanges();

        DbEntityEntry Entry(object entity);

        DbSet<FileContent> Files { get; set; }
    }
}
