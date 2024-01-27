using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StructureHelper.Windows.CalculationWindows.ProgressViews
{
    /// <summary>
    /// Логика взаимодействия для TraceDocumentView.xaml
    /// </summary>
    public partial class TraceDocumentView : Window
    {
        TraceDocumentVM viewModel;
        public TraceDocumentView(TraceDocumentVM viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
            this.viewModel.DocumentReader = this.DocumentReader;
            this.viewModel.Show();
        }
        public TraceDocumentView(IEnumerable<ITraceLoggerEntry> loggerEntries) : this(new TraceDocumentVM(loggerEntries)) { }
    }
}
