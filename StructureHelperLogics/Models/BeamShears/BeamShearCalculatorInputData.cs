using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class BeamShearCalculatorInputData : IBeamShearCalculatorInputData
    {
        /// <inheritdoc/>
        public Guid Id { get; }


        /// <inheritdoc/>
        public List<IBeamShearAction> BeamShearActions { get; } = new();
        /// <inheritdoc/>
        public List<IBeamShearSection> ShearSections { get; } = new();
        /// <inheritdoc/>
        public List<IStirrup> Stirrups { get; } = new();
        public BeamShearCalculatorInputData(Guid id)
        {
            Id = id;
        }

    }
}
