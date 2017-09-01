using Autofac;
using Autofac.Builder;
using Autofac.Core;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.DependencyInjection;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.StringTools;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using System.Windows;

namespace CommonTools.Lib.fx45.DependencyInjection
{
    public static class AutofacExtensions
    {
        public static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> MainWindow<T>(this ContainerBuilder buildr)
            where T : MainWindowVmBase, IComponentResolver
            => buildr.RegisterType<T>().As<IComponentResolver>()
                                       .As<MainWindowVmBase>()
                                       .AsSelf().SingleInstance();


        public static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> Solo<T>(this ContainerBuilder buildr)
            => buildr.RegisterType<T>().AsSelf().SingleInstance();

        public static IRegistrationBuilder<TConcrete, ConcreteReflectionActivatorData, SingleRegistrationStyle> Solo<TInterface, TConcrete>(this ContainerBuilder buildr) where TConcrete : TInterface
            => buildr.RegisterType<TConcrete>().As<TInterface>().SingleInstance();

        public static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> Multi<T>(this ContainerBuilder buildr)
            => buildr.RegisterType<T>().AsSelf();

        public static IRegistrationBuilder<TConcrete, ConcreteReflectionActivatorData, SingleRegistrationStyle> Multi<TInterface, TConcrete>(this ContainerBuilder buildr) where TConcrete : TInterface
            => buildr.RegisterType<TConcrete>().As<TInterface>();


        public static IRegistrationBuilder<IHubContext, SimpleActivatorData, SingleRegistrationStyle> Hub<THub>(this ContainerBuilder buildr, HubConfiguration hubCfg)
            where THub : Hub
        {
            buildr.RegisterType<THub>().ExternallyOwned();

            return buildr.Register(_ => hubCfg.Resolver.Resolve<IConnectionManager>()
                .GetHubContext<THub>()).ExternallyOwned();
        }


        public static IRegistrationBuilder<IHubContext<TClient>, SimpleActivatorData, SingleRegistrationStyle> Hub<THub, TClient>(this ContainerBuilder buildr, HubConfiguration hubCfg) 
            where THub    : Hub<TClient>
            where TClient : class
        {
            buildr.RegisterType<THub>().ExternallyOwned();

            return buildr.Register(_ => hubCfg.Resolver.Resolve<IConnectionManager>()
                .GetHubContext<THub, TClient>()).ExternallyOwned();
        }


        public static void ShowMainWindow<T>(this ILifetimeScope scope) 
            where T : Window, new()
        {
            if (!scope.TryResolveOrAlert<MainWindowVmBase>(out MainWindowVmBase vm))
            {
                CurrentExe.Shutdown();
                return;
            }
            var win = new T();
            vm.HandleWindowEvents(win, scope);
            win.Show();
        }


        public static bool TryResolveOrAlert<T>(this ILifetimeScope scope, out T component)
        {
            if (scope == null) goto ReturnFalse;
            try
            {
                component = scope.Resolve<T>();
                return true;
            }
            catch (DependencyResolutionException ex)
            {
                MessageBox.Show(ex.GetMessage(), "Failed to Resolve Dependencies", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ReturnFalse:
                component = default(T);
                return false;
        }



        private static string GetMessage(this DependencyResolutionException ex)
        {
            if (ex.InnerException == null)
                return ex.Message;

            if (ex.InnerException.InnerException == null)
                return ex.InnerException.Message;

            var msg = ex.InnerException.InnerException.Message;

            if (msg.Contains("DefaultConstructorFinder"))
            {
                var resolving = msg.Between("DefaultConstructorFinder' on type '", "'");
                var argTyp = msg.Between("Cannot resolve parameter '", " ");
                var argNme = msg.Between(argTyp + " ", "'");
                return $"Check constructor of :{L.f}‹{resolving}›{L.F}"
                     + $"Can't resolve argument “{argNme}” of type :{L.f}‹{argTyp}›";
            }
            else
            {
                return ex.InnerException.InnerException.Info(false, true);
            }
        }
    }
}
