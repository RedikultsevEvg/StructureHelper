using StructureHelper.Windows.Forces;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.BeamShears
{
    public class DistributedLoadViewModel : OkCancelViewModelBase
    {
        private readonly IDistributedLoad distributedLoad;

        public double LoadValue
        {
            get => distributedLoad.LoadValue.Qy;
            set
            {
                distributedLoad.LoadValue.Qy = value;
                OnPropertyChanged(nameof(LoadValue));
            }
        }
        
        public FactoredCombinationPropertyVM CombinationProperty { get; }
        public DistributedBeamSpanLoadViewModel DistributedProperty { get; }

        public DistributedLoadViewModel(IDistributedLoad distributedLoad)
        {
            this.distributedLoad = distributedLoad;
            CombinationProperty = new(this.distributedLoad.CombinationProperty);
            DistributedProperty = new(this.distributedLoad);
        }
    }
}
