using StructureHelper.Windows.Forces;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Forces;

namespace StructureHelper.Windows.BeamShears
{
    public class TrapezoidLoadViewModel : OkCancelViewModelBase
    {
        private ITrapezoidDistributedLoad trapezoidLoad;

        public double StartValue
        {
            get => trapezoidLoad.StartLoadValue.Qy;
            set
            {
                trapezoidLoad.StartLoadValue.Qy = value;
                OnPropertyChanged(nameof(StartValue));
            }
        }

        public double EndValue
        {
            get => trapezoidLoad.EndLoadValue.Qy;
            set
            {
                trapezoidLoad.EndLoadValue.Qy = value;
                OnPropertyChanged(nameof(EndValue));
            }
        }

        public FactoredCombinationPropertyVM CombinationProperty { get; }
        public DistributedBeamSpanLoadViewModel DistributedProperty { get; }

        public TrapezoidLoadViewModel(ITrapezoidDistributedLoad trapezoidLoad)
        {
            this.trapezoidLoad = trapezoidLoad;
            CombinationProperty = new(this.trapezoidLoad.CombinationProperty);
            DistributedProperty = new(this.trapezoidLoad);
        }

    }
}
