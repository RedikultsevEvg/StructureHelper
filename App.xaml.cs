using System.Windows;
using Autofac;
using StructureHelper.Services;
using StructureHelper.Services.Primitives;
using StructureHelper.UnitSystem;
using StructureHelper.Windows.MainWindow;
using StructureHelperLogics.Services;
using StructureHelperLogics.Services.NdmCalculations;

namespace StructureHelper
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IContainer Container { get; private set; }
        public static ILifetimeScope Scope { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var builder = new ContainerBuilder();
            builder.RegisterType<PrimitiveRepository>().As<IPrimitiveRepository>().SingleInstance();
            builder.RegisterType<UnitSystemService>().AsSelf().SingleInstance();
            builder.RegisterType<CalculationService>().AsSelf().SingleInstance();
            builder.RegisterType<MainModel>().AsSelf().SingleInstance();
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();

            builder.RegisterType<MainView>().AsSelf();

            Container = builder.Build();
            Scope = Container.Resolve<ILifetimeScope>();

            var window = Scope.Resolve<MainView>();
            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Scope.Dispose();
            base.OnExit(e);
        }
    }
}
