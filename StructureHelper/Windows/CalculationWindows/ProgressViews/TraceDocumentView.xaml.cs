using StructureHelperCommon.Models;
using System.Collections.Generic;
using System.Windows;

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
