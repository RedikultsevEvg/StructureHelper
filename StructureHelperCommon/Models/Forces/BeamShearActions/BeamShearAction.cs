using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class BeamShearAction : IBeamShearAction
    {
        private IUpdateStrategy<IBeamShearAction> updateStrategy;
        public Guid Id { get; }
        public string Name { get; set; }
        public IBeamShearAxisAction XAxisShearAction { get; } = new BeamShearAxisAction(Guid.NewGuid());

        public IBeamShearAxisAction YAxisShearAction { get; } = new BeamShearAxisAction(Guid.NewGuid());

        public BeamShearAction(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            BeamShearAction beamShearAction = new(Guid.NewGuid());
            updateStrategy ??= new BeamShearActionUpdateStrategy();
            updateStrategy.Update(beamShearAction, this);
            return beamShearAction;
        }
    }
}
