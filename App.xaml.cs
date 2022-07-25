using System.Windows;
using Autofac;
using StructureHelper.Services;
using StructureHelper.Windows.MainWindow;

namespace StructureHelper
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IContainer Container { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<PrimitiveRepository>().As<IPrimitiveRepository>();
            builder.RegisterType<PrimitiveService>().As<IPrimitiveService>();
            builder.RegisterType<MainModel>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            builder.RegisterType<MainView>().AsSelf();

            Container = builder.Build();
            
            using (var scope = Container.BeginLifetimeScope())
            {
                var window = scope.Resolve<MainView>();
                window.ShowDialog();
            }
        }
    }
}
