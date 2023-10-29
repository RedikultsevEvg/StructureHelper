using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    /// Логика взаимодействия для MultiplyTuple.xaml
    /// </summary>
    public partial class MultiplyTuple : UserControl
    {
        public event EventHandler ValueChanged;

        public MultiplyTuple()
        {
            InitializeComponent();
            //DataContext = this;
        }

        public ICommand MultiplyByFactor
        {
            get => muliplyByFactor ??= new RelayCommand(o =>
            {
                try
                {
                    string s = (string)o;
                    double factor = CommonOperation.ConvertToDoubleChangeComma(s);
                    ChangeValue(factor);
                }
                catch(Exception ex)
                {

                }
            });
        }


        public static readonly DependencyProperty ForceTupleProperty = DependencyProperty.Register(
        "ForceTuple", typeof(ForceTuple), typeof(MultiplyTuple), new PropertyMetadata(new ForceTuple()));

        private RelayCommand muliplyByFactor;

        public ForceTuple ForceTuple
        {
            get { return (ForceTuple)GetValue(ForceTupleProperty); }
            set { SetValue(ForceTupleProperty, value); }
        }

        private void Multy0_5_Click(object sender, RoutedEventArgs e)
        {
            ChangeValue(0.5d);
        }
        private void Multy2_0_Click(object sender, RoutedEventArgs e)
        {
            ChangeValue(2d);
        }

        private void ChangeValue(double factor)
        {
            var tmpTuple = ForceTupleService.MultiplyTuples(ForceTuple, factor);
            ForceTupleService.CopyProperties(tmpTuple, ForceTuple, 1d);
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Multy0_Click(object sender, RoutedEventArgs e)
        {
            ChangeValue(0d);
        }

        private void Multy0_12_Click(object sender, RoutedEventArgs e)
        {
            ChangeValue(0.1d);
        }

        private void MultyM1_2_Click(object sender, RoutedEventArgs e)
        {
            ChangeValue(-1d);
        }

        private void Multy10_02_Click(object sender, RoutedEventArgs e)
        {
            ChangeValue(10d);
        }
    }
}
