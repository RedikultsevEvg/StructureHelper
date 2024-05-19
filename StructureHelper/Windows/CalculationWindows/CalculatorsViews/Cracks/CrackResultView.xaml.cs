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
    /// Логика взаимодействия для CrackResultView.xaml
    /// </summary>
    public partial class CrackResultView : Window
    {
        private readonly CrackResultViewModel viewModel;

        public CrackResultView(CrackResultViewModel viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();
            this.DataContext = this.viewModel;
        }
        public CrackResultView(CrackResult result) : this(new CrackResultViewModel(result))
        {
            
        }
    }
}
