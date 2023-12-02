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
    public partial class MultiplyDouble : UserControl
    {
        public event EventHandler ValueChanged;

        public MultiplyDouble()
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



        public double DoubleValue
        {
            get { return (double)GetValue(DoubleValueProperty); }
            set { SetValue(DoubleValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DoubleValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DoubleValueProperty =
            DependencyProperty.Register("DoubleValue", typeof(double), typeof(MultiplyDouble), new PropertyMetadata(new double()));



        public static readonly DependencyProperty DoubleFactorProperty = DependencyProperty.Register(
        "DoubleFactor", typeof(double), typeof(MultiplyDouble), new PropertyMetadata(new double()));

        private RelayCommand muliplyByFactor;

        public double DoubleFactor
        {
            get { return (double)GetValue(DoubleFactorProperty); }
            set { SetValue(DoubleFactorProperty, value); }
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
            DoubleValue *= factor;
            DoubleFactor = factor;
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
