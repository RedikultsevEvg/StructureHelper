using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class UniformlyDistributedLoad : IUniformlyDistributedLoad
    {
        private double relativeLoadLevel;

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

        public UniformlyDistributedLoad(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
