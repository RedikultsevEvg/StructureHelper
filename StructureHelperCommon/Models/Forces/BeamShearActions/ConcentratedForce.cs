using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ConcentratedForce : IConcenratedForce
    {
        private double relativeLoadLevel;

        /// <inheritdoc/>
        public Guid Id { get; set; }
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public double ForceCoordinate { get; set; }
        /// <inheritdoc/>
        public double ForceValue { get; set; }
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

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
