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

namespace StructureHelper.Windows.LoadHistory
{
    /// <summary>
    /// Логика взаимодействия для HistoryCalculatorView.xaml
    /// </summary>
    public partial class HistoryCalculatorView : Window
    {
        private HistoryCalculatorViewModel vm;

        public HistoryCalculatorView(HistoryCalculatorViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            vm.ParentWindow = this;
            this.DataContext = vm;
        }
    }
}
