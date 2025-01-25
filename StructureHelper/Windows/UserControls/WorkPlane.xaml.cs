using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.MainWindow;
using StructureHelper.Windows.ViewModels.Materials;
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

namespace StructureHelper.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for WorkPlane.xaml
    /// </summary>
    public partial class WorkPlane : UserControl
    {


        public CrossSectionViewModel ViewModel
        {
            get { return (CrossSectionViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(CrossSectionViewModel), typeof(WorkPlane), new PropertyMetadata(null));



        public WorkPlane()
        {
            InitializeComponent();
        }

        private void ContentPresenter_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var contentPresenter = sender as ContentPresenter;
            var item = contentPresenter?.Content as PrimitiveBase;
            ViewModel.PrimitiveLogic.SelectedItem = item;
        }
    }
}
