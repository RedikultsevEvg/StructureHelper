using StructureHelper.Infrastructure;
using StructureHelper.Windows.Forces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearAxisActionViewModel : ViewModelBase
    {
        private readonly IBeamShearAxisAction beamShearAxisAction;

        public double SupportShearForce
        {
            get => beamShearAxisAction.SupportShearForce;
            set
            {
                beamShearAxisAction.SupportShearForce = value;
            }
        }
        public FactoredCombinationPropertyVM CombinationProperty { get; }
        public BeamShearLoadsViewModel ShearLoads { get; }

        public BeamShearAxisActionViewModel(IBeamShearAxisAction beamShearAxisAction)
        {
            this.beamShearAxisAction = beamShearAxisAction;
            CombinationProperty = new(this.beamShearAxisAction.FactoredCombinationProperty);
            ShearLoads = new(this.beamShearAxisAction.ShearLoads);
        }
    }
}
