using StructureHelper.Windows.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    /// <summary>
    /// Логика взаимодействия для SourceToTargetControl.xaml
    /// </summary>
    public partial class SourceToTargetControl : UserControl
    {
        //private ISourceToTargetViewModel<TItem> viewModel;

        public SourceToTargetControl()
            //ISourceToTargetViewModel<TItem> viewModel)
        {
            //this.viewModel = viewModel;
            InitializeComponent();
            //DataContext = this.viewModel;
        }
    }
}
