using StructureHelperCommon.Models.Analyses;
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

namespace StructureHelper.Windows.MainWindow.Analyses
{
    /// <summary>
    /// Interaction logic for AnalysisView.xaml
    /// </summary>
    public partial class AnalysisView : Window
    {
        private readonly AnalysisViewModel viewModel;

        public AnalysisView(AnalysisViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
            this.DataContext = this.viewModel;
            InitializeComponent();
        }
        public AnalysisView(IAnalysis analysis) : this(new AnalysisViewModel(analysis))
        {
        }

        public AnalysisView(IVisualAnalysis visualAnalysis) : this(visualAnalysis.Analysis)
        {
            
        }
    }
}
