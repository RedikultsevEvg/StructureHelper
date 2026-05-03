using StructureHelper.Infrastructure;
using StructureHelper.Windows.Forces;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelperCommon.Models.Forces;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearAxisActionViewModel : ViewModelBase
    {
        private readonly IBeamShearAxisAction beamShearAxisAction;

        /// <summary>
        /// Factored tuple of force at support of beam ander shear
        /// </summary>
        public FactoredForceTupleViewModel SupportForces { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public BeamShearLoadsViewModel ShearLoads { get; }

        public BeamShearAxisActionViewModel(IBeamShearAxisAction beamShearAxisAction)
        {
            this.beamShearAxisAction = beamShearAxisAction;
            SupportForces = new(this.beamShearAxisAction.SupportForce);
            ShearLoads = new(this.beamShearAxisAction.ShearLoads);
        }
    }
}
