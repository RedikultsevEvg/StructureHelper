using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    /// <inheritdoc/>
    public class BeamShearAxisAction : IBeamShearAxisAction
    {
        private IUpdateStrategy<IBeamShearAxisAction> updateStrategy;
        ///<inheritdoc/>
        public Guid Id { get; }
        ///<inheritdoc/>
        public string Name { get; set; } = string.Empty;
        ///<inheritdoc/>
        public double SupportShearForce { get; set; }
        ///<inheritdoc/>
        public IFactoredCombinationProperty FactoredCombinationProperty { get; } = new FactoredCombinationProperty(Guid.NewGuid());
        ///<inheritdoc/>
        public List<IBeamShearLoad> ShearLoads { get; }


        public BeamShearAxisAction(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            BeamShearAxisAction beamShearAxisAction = new(Guid.NewGuid());
            return beamShearAxisAction;
        }
    }
}
