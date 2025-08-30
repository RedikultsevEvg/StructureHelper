using StructureHelperLogics.Models.BeamShears;
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

namespace StructureHelper.Windows.BeamShears
{
    /// <summary>
    /// Логика взаимодействия для InclinedSectionViewerView.xaml
    /// </summary>
    public partial class InclinedSectionViewerView : Window
    {
        private InclinedSectionViewerViewModel viewModel;
        public InclinedSectionViewerView(InclinedSectionViewerViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
            InitializeComponent();
        }
        public InclinedSectionViewerView(IBeamShearSectionLogicResult sectionResult) : this(new InclinedSectionViewerViewModel(sectionResult))
        {
            
        }
    }
}
