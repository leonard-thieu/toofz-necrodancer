using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Leaderboards.EntityFramework;
using toofz.NecroDancer.Web.Api;
using toofz.NecroDancer.Web.Api._Leaderboards;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace toofz.NecroDancer.Web.Api
{
    [ExcludeFromCodeCoverage]
    internal static class NinjectWebCommon
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
                kernel.Bind<System.Web.IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
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
        private static void RegisterServices(IKernel kernel)
        {
            var leaderboardsConnectionString = Util.GetEnvVar("LeaderboardsConnectionString");

            kernel.Bind<LeaderboardsContext>().ToConstructor(s => new LeaderboardsContext(leaderboardsConnectionString));
            kernel.Bind<ILeaderboardsSqlClient>().ToConstructor(s => new LeaderboardsSqlClient(leaderboardsConnectionString));
            kernel.Bind<LeaderboardsService>().ToSelf();
            kernel.Bind<IHttpServerUtilityWrapper>().ToConstructor(s => new HttpServerUtilityWrapper());
        }
    }
}
