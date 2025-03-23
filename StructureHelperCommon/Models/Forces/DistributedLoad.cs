using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class DistributedLoad : IDistributedLoad
    {
        private double relativeLoadLevel;
        private IUpdateStrategy<IDistributedLoad> updateStrategy;

        public Guid Id { get; }

        public string Name { get; set; } = string.Empty;
        public double LoadValue { get; set; } = 0d;
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

        public double StartCoordinate { get; set; } = double.NegativeInfinity;
        public double EndCoordinate { get; set; } = double.PositiveInfinity;
        /// <inheritdoc/>
        public double LoadRatio { get; set; } = 1;

        public DistributedLoad(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            DistributedLoad distributedLoad = new(Guid.NewGuid());
            updateStrategy ??= new DistributedLoadUpdateStrategy();
            updateStrategy.Update(distributedLoad, this);
            return distributedLoad;
        }
    }
}
