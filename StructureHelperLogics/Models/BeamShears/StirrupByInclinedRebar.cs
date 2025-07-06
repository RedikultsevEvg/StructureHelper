using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByInclinedRebar : IStirrupByInclinedRebar
    {
        private const int minInclinationAngle = 10;
        private const int maxInclinationAngle = 80;
        private double angleOfInclination = 45;
        private double startCoordinate = 0.05;
        private double compressedGap = 0.05;

        /// <inheritdoc>
        public Guid Id { get; }
        public string? Name { get; set; } = string.Empty;
        /// <inheritdoc>
        public double CompressedGap
        {
            get => compressedGap;
            set
            {
                if (value < 0)
                {
                    throw new StructureHelperException(ErrorStrings.IncorrectValue + $": distance from compressed edge must be positive");
                }
                compressedGap = value;
            }
        }
        /// <inheritdoc>
        public double StartCoordinate
        {
            get => startCoordinate;
            set
            {
                if (value < 0)
                {
                    throw new StructureHelperException(ErrorStrings.IncorrectValue + $": start coordinate must be positive");
                }
                startCoordinate = value;
            }
        }
        /// <inheritdoc>
        public double OffSet { get; set; } = 0.05;
        /// <inheritdoc>
        public double AngleOfInclination
        {
            get => angleOfInclination;
            set
            {
                if (value < minInclinationAngle || value > maxInclinationAngle)
                {
                    throw new StructureHelperException(ErrorStrings.IncorrectValue + $": angle of inclination of rebar must be beetwen {minInclinationAngle} and {maxInclinationAngle} degrees");
                }
                angleOfInclination = value;
            }
        }

        public StirrupByInclinedRebar(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            var logic = new StirrupByInclinedRebarUpdateStrategy();
            StirrupByInclinedRebar newItem = new(Guid.NewGuid());
            logic.Update(newItem, this);
            return newItem;
        }
    }
}
