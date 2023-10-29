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

namespace StructureHelper.Windows.Arrays
{
    /// <summary>
    /// Логика взаимодействия для ArrayView.xaml
    /// </summary>
    public partial class ArrayView : Window
    {
        private ArrayViewModel viewModel;
        public ArrayView(ArrayViewModel vm)
        {
            InitializeComponent();
            viewModel = vm;
            DataContext = viewModel;      
        }

        private void ArrayTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //if (e.NewValue is selectedItem)
            //{
            //    viewModel.SelectedItem = selectedItem;
            //}
        }
    }
}
