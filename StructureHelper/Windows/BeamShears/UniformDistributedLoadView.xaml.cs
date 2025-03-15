using StructureHelperCommon.Models.Forces;
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
    /// Interaction logic for UniformDistributedLoadView.xaml
    /// </summary>
    public partial class UniformDistributedLoadView : Window
    {
        private readonly UniformDistributedLoadViewModel viewModel;
        public UniformDistributedLoadView(UniformDistributedLoadViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
            this.viewModel.ParentWindow = this;
        }

        public UniformDistributedLoadView(IDistributedLoad distributedLoad) : this(new UniformDistributedLoadViewModel(distributedLoad))
        {
            
        }
    }
}
