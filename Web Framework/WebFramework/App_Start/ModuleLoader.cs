using log4net;
using Ninject;
using Ninject.Modules;
using System.Reflection;
using System.Configuration;
using WebFramework.Helpers;
using WebFramework.Persistence.Blog;
using WebFramework.Persistence.GDrive;
using WebFramework.Persistence.Products;

namespace WebFramework.App_Start
{
    public class ModuleLoader : NinjectModule
    {
        public override void Load()
        {
            Bind<ILog>().ToConstant<ILog>(LogManager.GetLogger(Constants.ServerLoggerName)).Named(Constants.ServerLoggerName);
            Bind<ILog>().ToConstant<ILog>(LogManager.GetLogger(Constants.ClientLoggerName)).Named(Constants.ClientLoggerName);

            Bind<IProductsDbContext>().To<ProductsDbContext>();
            Bind<IBlogDbContext>().To<BlogDbContext>();
            Bind<IGDriveDbContext>().To<GDriveDbContext>();
        }

        private static readonly StandardKernel _instance = CreateInstance();
        public static StandardKernel DI { get { return _instance; } }

        private static StandardKernel CreateInstance()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }
    }
}