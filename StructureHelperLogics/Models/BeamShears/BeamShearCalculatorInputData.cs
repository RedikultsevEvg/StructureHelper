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
        public List<IBeamShearAction> Actions { get; } = new();
        /// <inheritdoc/>
        public List<IBeamShearSection> Sections { get; } = new();
        /// <inheritdoc/>
        public List<IStirrup> Stirrups { get; } = new();
        public IBeamShearDesignRangeProperty DesignRangeProperty { get; set; } = new BeamShearDesignRangeProperty(Guid.NewGuid());

        public BeamShearCalculatorInputData(Guid id)
        {
            Id = id;
        }

        public void DeleteAction(IBeamShearAction action)
        {
            //nothing to do
        }

        public void DeleteSection(IBeamShearSection section)
        {
            //nothing to do
        }

        public void DeleteStirrup(IStirrup stirrup)
        {
            //nothing to do
        }
    }
}
