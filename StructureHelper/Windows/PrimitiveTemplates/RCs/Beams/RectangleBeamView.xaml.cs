using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels.PrimitiveTemplates.RCs;
using StructureHelperLogics.Models.Templates.RCs;
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

namespace StructureHelper.Windows.PrimitiveTemplates.RCs.RectangleBeam
{
    /// <summary>
    /// Логика взаимодействия для RectangleBeamView.xaml
    /// </summary>
    public partial class RectangleBeamView : Window
    {
        private RectangleBeamViewModel viewModel;

        public IRectangleBeamTemplate ResultModel { get; set; }

        public RectangleBeamView(IRectangleBeamTemplate template)
        {
            viewModel = new RectangleBeamViewModel(template);
            viewModel.ParentWindow = this;
            this.DataContext = viewModel;
            InitializeComponent();
        }
    }
}
