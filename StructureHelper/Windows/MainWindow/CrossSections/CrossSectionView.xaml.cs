using System.Windows;
using System.Windows.Controls;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services;
using StructureHelper.Services.Primitives;
using StructureHelperLogics.Models.CrossSections;

namespace StructureHelper.Windows.MainWindow
{
    public partial class CrossSectionView : Window
    {
        private CrossSectionViewModel viewModel;
        //public IPrimitiveRepository PrimitiveRepository { get; }

        public CrossSectionView(CrossSectionViewModel viewModel)
        {
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            InitializeComponent();
        }

        public CrossSectionView(ICrossSection crossSection) : this(new CrossSectionViewModel(crossSection))
        {
            
        }

        private void ContentPresenter_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var contentPresenter = sender as ContentPresenter;
            var item = contentPresenter?.Content as PrimitiveBase;
            viewModel.PrimitiveLogic.SelectedItem = item;
        }
    }
}
