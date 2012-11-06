using System;
using System.Configuration;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Syntax;
using Ninject.Web.Common;
using Raven.Client;
using Raven.Client.Document;
using dogalog.App_Start;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace dogalog.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public static void RegisterServices(IBindingRoot kernel)
        {
            var url = ConfigurationManager.AppSettings["RAVENHQ_CONNECTION_STRING"].Split('=')[1];

            if(url.ToUpper().IndexOf("APPHARBOR", System.StringComparison.Ordinal)!=-1)
            {
                url = String.Format("{0}; ApiKey=067d4f3d-d26a-4dc9-b362-e04a5a92ee8a", url);
            }
            kernel.Bind<IDocumentStore>()
                   .ToMethod(context =>
                   {
                       var documentStore = new DocumentStore
                           {
                               Url = "https://1.ravenhq.com/databases/AppHarbor_7a9dd280-17f7-439b-aea4-36467fd9ef1d",
                               ApiKey = "067d4f3d-d26a-4dc9-b362-e04a5a92ee8a"
                           };
                       return documentStore.Initialize();
                   })
                   .InTransientScope();

            kernel.Bind<IDocumentSession>().ToMethod(context => context.Kernel.Get<IDocumentStore>().OpenSession()).InRequestScope();
        }
    }
}
