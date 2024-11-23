using StructureHelperCommon.Models.Analyses;
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

namespace StructureHelper.Windows.MainWindow.Analyses
{
    /// <summary>
    /// Interaction logic for VersionsView.xaml
    /// </summary>
    public partial class VersionsView : Window
    {
        VersionsViewModel viewModel;
        public VersionsView(VersionsViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            if (this.viewModel.OwnerWindow is not null)
            {
                Owner = this.viewModel.OwnerWindow;
            }
        }

        public VersionsView(IVersionProcessor versionProcessor) : this (new VersionsViewModel(versionProcessor))
        {
            
        }
    }
}
