using StructureHelper.Windows.UserControls;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StructureHelper.Windows.Forces
{
    /// <summary>
    /// Логика взаимодействия для InterpolateTuplesView.xaml
    /// </summary>
    public partial class InterpolateTuplesView : Window
    {
        InterpolateTuplesViewModel viewModel;
        public InterpolateTuplesView(InterpolateTuplesViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = this.viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void StartValueChanged(object sender, EventArgs e)
        {
            var obj = (MultiplyDouble)sender;
            var tmpTuple = ForceTupleService.MultiplyTuples(viewModel.StartDesignForce.ForceTuple, obj.DoubleFactor);
            ForceTupleService.CopyProperties(tmpTuple, viewModel.StartDesignForce.ForceTuple, 1d);
            viewModel.RefreshStartTuple();
        }

        private void FinishValueChanged(object sender, EventArgs e)
        {
            var obj = (MultiplyDouble)sender;
            var tmpTuple = ForceTupleService.MultiplyTuples(viewModel.FinishDesignForce.ForceTuple, obj.DoubleFactor);
            ForceTupleService.CopyProperties(tmpTuple, viewModel.FinishDesignForce.ForceTuple, 1d);
            viewModel.RefreshFinishTuple();
        }

        private void StepCountValueChanged(object sender, EventArgs e)
        {
            var obj = (MultiplyDouble)sender;
            var factor = obj.DoubleFactor;
            if (factor > 0d)
            {
                viewModel.StepCount = Convert.ToInt32(viewModel.StepCount * factor);
            }
        }
    }
}
