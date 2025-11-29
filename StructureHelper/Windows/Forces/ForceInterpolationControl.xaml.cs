using StructureHelper.Windows.UserControls;
using StructureHelperCommon.Services.Forces;
using System;
using System.Windows.Controls;

namespace StructureHelper.Windows.Forces
{
    /// <summary>
    /// Логика взаимодействия для ForceInterpolationControl.xaml
    /// </summary>
    public partial class ForceInterpolationControl : UserControl
    {
        private ForceTupleInterpolationViewModel? properties;
        private IForceTupleServiceLogic forceTupleServiceLogic;
        private IForceTupleServiceLogic ForceTupleServiceLogic => forceTupleServiceLogic ??= new ForceTupleServiceLogic();

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
            var tmpTuple = ForceTupleServiceLogic.MultiplyTupleByFactor(Properties.StartDesignForce, obj.DoubleFactor);
            ForceTupleServiceLogic.CopyProperties(tmpTuple, Properties.StartDesignForce, 1d);
            Properties.RefreshStartTuple();
        }

        private void FinishValueChanged(object sender, EventArgs e)
        {
            var obj = (MultiplyDouble)sender;
            var tmpTuple = ForceTupleServiceLogic.MultiplyTupleByFactor(Properties.FinishDesignForce, obj.DoubleFactor);
            ForceTupleServiceLogic.CopyProperties(tmpTuple, Properties.FinishDesignForce, 1d);
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
