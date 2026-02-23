using StructureHelperCommon.Models.FeaMaterials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StructureHelper.Windows.FeaMaterials
{
    /// <summary>
    /// Interaction logic for FeaMaterialsView.xaml
    /// </summary>
    public partial class FeaMaterialsView : Window
    {
        private FeaMaterialsViewModel viewModel;
        public FeaMaterialsView(FeaMaterialsViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
        }

        public FeaMaterialsView(IFeaMaterialRepository feaMaterialRepository) : this(new FeaMaterialsViewModel(feaMaterialRepository)) { }

        public IFeaMaterialRepository FeaMaterialRepository { get; }
    }
}
