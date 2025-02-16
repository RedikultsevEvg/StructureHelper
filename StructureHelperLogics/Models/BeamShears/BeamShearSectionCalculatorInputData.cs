using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class BeamShearSectionCalculatorInputData : IBeamShearSectionCalculatorInputData
    {
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public IBeamShearSection? BeamShearSection { get; set; }
        /// <inheritdoc/>
        public IBeamShearAction? BeamShearAction { get; set; }
        /// <inheritdoc/>
        public IStirrup? Stirrup { get; set; }
        public BeamShearSectionCalculatorInputData(Guid id)
        {
            Id = id;
        }

    }
}
