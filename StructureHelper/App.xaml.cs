using StructureHelper.Services.Primitives;
using StructureHelper.UnitSystem;
using StructureHelper.Windows.MainWindow;
using StructureHelperLogics.Services.NdmCalculations;
using System.Windows;
using Autofac;
using System;
using System.IO;
using StructureHelperCommon.Infrastructures.Settings;

namespace StructureHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
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
            //builder.RegisterType<CrossSectionModel>().AsSelf().SingleInstance();
            //builder.RegisterType<CrossSectionViewModel>().AsSelf().SingleInstance();
            //builder.RegisterType<CrossSectionView>().AsSelf();

            builder.RegisterType<AnalysesManagerViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<AnalysesManagerView>().AsSelf();

            Container = builder.Build();
            Scope = Container.Resolve<ILifetimeScope>();
            // Get the command-line arguments
            string[] args = Environment.GetCommandLineArgs();

            // Check if there is an argument provided for a file
            if (args.Length > 1) // args[0] is the application path
            {
                OpenSpecifiedFile(args);
            }
            else
            {
                // No file specified, open normally
                var window = new AnalysesManagerView();
                window.Show();
            }
            SettingsSerializer.LoadSettings();
        }

        private static void OpenSpecifiedFile(string[] args)
        {
            string filePath = args[1];

            // Optional: Validate the file path
            if (File.Exists(filePath))
            {
                // Pass the file path to your main window or handling logic
                var vm = new AnalysesManagerViewModel();
                vm.FileLogic.OpenNewFile(filePath);
                //var window = Scope.Resolve<AnalysesManagerView>(vm);
                var window = new AnalysesManagerView(vm);
                window.Show();
            }
            else
            {
                MessageBox.Show("File does not exist.");
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                SettingsSerializer.SaveSettings();
            }
            catch (Exception ex)
            {

            }
            Scope.Dispose();
            base.OnExit(e);
        }
    }
}
