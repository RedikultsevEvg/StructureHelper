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

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    /// <summary>
    /// Логика взаимодействия для InteractionDiagramCalculatorView.xaml
    /// </summary>
    public partial class LimitCurveCalculatorView : Window
    {
        LimitCurveCalculatorViewModel viewModel;
        public LimitCurveCalculatorView(LimitCurveCalculatorViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
            InitializeComponent();
            this.DataContext = this.viewModel;
            CurveData.LimitCurveViewModel = this.viewModel.LimitCurveDataViewModel;
        }
    }
}
