using NLog;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ConcentratedForce : IConcentratedForce
    {
        private double relativeLoadLevel;
        private IUpdateStrategy<IConcentratedForce>? updateStrategy;

        /// <inheritdoc/>
        public Guid Id { get;}


        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public double ForceCoordinate { get; set; }
        /// <inheritdoc/>
        public IForceTuple ForceValue { get; set; } = new ForceTuple(Guid.NewGuid());
        /// <inheritdoc/>
        public double RelativeLoadLevel
        {
            get => relativeLoadLevel;
            set
            {
                if (value > 0.5d) { relativeLoadLevel = 0.5d; }
                if (value < -0.5d) { relativeLoadLevel = -0.5d; }
                relativeLoadLevel = value;
            }
        }
        /// <inheritdoc/>
        public double LoadRatio { get; set; } = 1;
        /// <inheritdoc/>
        public IFactoredCombinationProperty CombinationProperty { get; set; } = new FactoredCombinationProperty(Guid.NewGuid()) { LimitState = LimitStates.ULS, CalcTerm = CalcTerms.ShortTerm };

        public ConcentratedForce(Guid id)
        {
            Id = id;
        }

        /// <inheritdoc/>
        public object Clone()
        {
            ConcentratedForce concentratedForce = new(Guid.NewGuid());
            updateStrategy ??= new ConcentratedForceUpdateStrategy();
            updateStrategy.Update(concentratedForce, this);
            return concentratedForce;
        }
    }
}
