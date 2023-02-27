using StructureHelper.Windows.ViewModels.PrimitiveTemplates.RCs;
using StructureHelperLogics.Models.Templates.CrossSections.RCs;
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

namespace StructureHelper.Windows.PrimitiveTemplates.RCs.Beams
{
    /// <summary>
    /// Логика взаимодействия для CircleView.xaml
    /// </summary>
    public partial class CircleView : Window
    {
        private CircleViewModel viewModel;

        public CircleView(ICircleTemplate template) 
        {
            viewModel = new CircleViewModel(template);
            viewModel.ParentWindow = this;
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
