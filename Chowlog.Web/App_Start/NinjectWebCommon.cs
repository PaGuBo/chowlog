[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Chowlog.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Chowlog.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Chowlog.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Chowlog.Web.App_Code;
    using Ninject.Modules;
    using System.Configuration;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                //RegisterServices(kernel);
                //return kernel;
                string moduleName = ConfigurationManager.AppSettings["InjectModule"];
                Type moduleType = Type.GetType(moduleName);

                if (moduleType != null)
                {
                    kernel.Load(Activator.CreateInstance(moduleType) as NinjectModule);
                }
                else
                {
                    throw new Exception(string.Format("Could not find Type: '{0}'", moduleName));
                }
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        //private static void RegisterServices(IKernel kernel)
        //{
        //}        
    }
    public class ProductionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFileUploadService>().To<AmazonFileUploadService>();
        }
    }

    public class DevelopmentModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFileUploadService>().To<LocalFileUploadService>();
        }
    }
}
