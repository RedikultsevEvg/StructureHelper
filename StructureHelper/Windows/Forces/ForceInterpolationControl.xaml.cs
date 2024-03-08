using StructureHelper.Windows.UserControls;
using StructureHelper.Windows.ViewModels.Materials;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StructureHelper.Windows.Forces
{
    /// <summary>
    /// Логика взаимодействия для ForceInterpolationControl.xaml
    /// </summary>
    public partial class ForceInterpolationControl : UserControl
    {
        private ForceTupleInterpolationViewModel? properties;

        public ForceTupleInterpolationViewModel? Properties
        {
            get => properties; set
            {
                properties = value;
                DataContext = Properties;
            }
        }
        public ForceInterpolationControl()
        {
            InitializeComponent();
        }

        private void StartValueChanged(object sender, EventArgs e)
        {
            var obj = (MultiplyDouble)sender;
            var tmpTuple = ForceTupleService.MultiplyTuples(Properties.StartDesignForce.ForceTuple, obj.DoubleFactor);
            ForceTupleService.CopyProperties(tmpTuple, Properties.StartDesignForce.ForceTuple, 1d);
            Properties.RefreshStartTuple();
        }

        private void FinishValueChanged(object sender, EventArgs e)
        {
            var obj = (MultiplyDouble)sender;
            var tmpTuple = ForceTupleService.MultiplyTuples(Properties.FinishDesignForce.ForceTuple, obj.DoubleFactor);
            ForceTupleService.CopyProperties(tmpTuple, Properties.FinishDesignForce.ForceTuple, 1d);
            Properties.RefreshFinishTuple();
        }

        private void StepCountValueChanged(object sender, EventArgs e)
        {
            var obj = (MultiplyDouble)sender;
            var factor = obj.DoubleFactor;
            if (factor > 0d)
            {
                Properties.StepCount = Convert.ToInt32(Properties.StepCount * factor);
            }
        }
    }
}
