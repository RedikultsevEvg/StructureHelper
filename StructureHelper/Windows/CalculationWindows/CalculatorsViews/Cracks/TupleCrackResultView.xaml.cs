using StructureHelperLogics.NdmCalculations.Cracking;
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

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    /// <summary>
    /// Логика взаимодействия для TupleCrackResultView.xaml
    /// </summary>
    public partial class TupleCrackResultView : Window
    {
        TupleCrackResultViewModel viewModel;
        public TupleCrackResultView(TupleCrackResultViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = this.viewModel;
        }
        public TupleCrackResultView(TupleCrackResult crackResult) : this(new TupleCrackResultViewModel(crackResult))
        {
            
        }
    }
}
