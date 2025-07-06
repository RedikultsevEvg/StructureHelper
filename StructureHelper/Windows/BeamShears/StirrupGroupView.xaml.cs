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
    /// Логика взаимодействия для StirrupGroupView.xaml
    /// </summary>
    public partial class StirrupGroupView : Window
    {
        StirrupGroupViewModel viewModel;
        public StirrupGroupView(StirrupGroupViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
            this.DataContext = this.viewModel;
        }
        public StirrupGroupView(IStirrupGroup stirrupGroup) : this(new StirrupGroupViewModel(stirrupGroup))
        {
            
        }
    }
}
