using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Forces;

namespace StructureHelper.Windows.BeamShears
{
    public class ConcentratedForceViewModel : OkCancelViewModelBase
    {
        private readonly IConcentratedForce concenratedForce;

        public double ForceCoordinate
        {
            get => concenratedForce.ForceCoordinate;
            set
            {
                concenratedForce.ForceCoordinate = value;
            }
        }
        public double ForceValue
        {
            get => concenratedForce.ForceValue;
            set
            {
                concenratedForce.ForceValue = value;
            }
        }
        public double LoadRatio
        {
            get => concenratedForce.LoadRatio;
            set
            {
                concenratedForce.LoadRatio = value;
            }
        }
        public double RelativeLevel
        {
            get => concenratedForce.RelativeLoadLevel;
            set
            {
                if (value > 0.5d)
                {
                    concenratedForce.RelativeLoadLevel = 0.5;
                    return;
                }
                if (value < -0.5d)
                {
                    concenratedForce.RelativeLoadLevel = -0.5;
                    return;
                }
                concenratedForce.RelativeLoadLevel = value;
            }
        }

        public ConcentratedForceViewModel(IConcentratedForce concenratedForce)
        {
            this.concenratedForce = concenratedForce;
        }
    }
}
