using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Forces;

namespace StructureHelper.Windows.BeamShears
{
    public class DistributedBeamSpanLoadViewModel : ViewModelBase
    {
        private IDistributedBeamSpanLoad distributedLoad;

        public string Name
        {
            get => distributedLoad.Name;
            set
            {
                distributedLoad.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public double LoadRatio
        {
            get => distributedLoad.LoadRatio;
            set
            {
                distributedLoad.LoadRatio = value;
                OnPropertyChanged(nameof(LoadRatio));
            }
        }

        public double RelativeLevel
        {
            get => distributedLoad.RelativeLoadLevel;
            set
            {
                if (value > 0.5d)
                {
                    distributedLoad.RelativeLoadLevel = 0.5;
                    return;
                }
                if (value < -0.5d)
                {
                    distributedLoad.RelativeLoadLevel = -0.5;
                    return;
                }
                distributedLoad.RelativeLoadLevel = value;
                OnPropertyChanged(nameof(RelativeLevel));
            }
        }
        public double StartCoordinate
        {
            get => distributedLoad.StartCoordinate;
            set
            {
                distributedLoad.StartCoordinate = value;
                OnPropertyChanged(nameof(StartCoordinate));
            }
        }
        public double EndCoordinate
        {
            get => distributedLoad.EndCoordinate;
            set
            {
                distributedLoad.EndCoordinate = value;
                OnPropertyChanged(nameof(EndCoordinate));
            }
        }

        public DistributedBeamSpanLoadViewModel(IDistributedBeamSpanLoad distributedLoad)
        {
            this.distributedLoad = distributedLoad;
        }
    }
}
