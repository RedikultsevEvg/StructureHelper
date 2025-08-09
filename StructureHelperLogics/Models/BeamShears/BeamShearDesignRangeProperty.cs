using StructureHelperCommon.Infrastructures.Exceptions;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc>
    public class BeamShearDesignRangeProperty : IBeamShearDesignRangeProperty
    {
        private const int minStepCount = 10;
        private const int maxStepCount = 1000;
        private double absoluteRangeValue = 0.0;
        private double relativeEffectiveDepthRangeValue = 3.3;
        private int stepCount = 55;
        private double relativeEffectiveDepthSectionLengthMaxValue = 3;
        private double relativeEffectiveDepthSectionLengthMinValue = 0;

        /// <inheritdoc>
        public Guid Id { get; }
        /// <inheritdoc>
        public double AbsoluteRangeValue
        {
            get => absoluteRangeValue;
            set
            {
                if (value < 0.0)
                {
                    throw new StructureHelperException($"Absolute value of beam shear design range must be positive, but was {value}");
                }
                absoluteRangeValue = value;
            }
        }
        /// <inheritdoc>
        public double RelativeEffectiveDepthRangeValue
        {
            get => relativeEffectiveDepthRangeValue;
            set
            {
                if (value < 0.0)
                {
                    throw new StructureHelperException($"Relative value of beam shear design range must be positive, but was {value}");
                }
                relativeEffectiveDepthRangeValue = value;
            }
        }

        public int StepCount
        {
            get => stepCount;
            set
            {
                if (value < minStepCount)
                {
                    throw new StructureHelperException($"Number of step design range must be greater or equal {minStepCount}, but was {value}");
                }
                if (value > maxStepCount)
                {
                    throw new StructureHelperException($"Number of step design range must be less or equal {maxStepCount}, but was {value}");
                }
                stepCount = value;
            }
        }

        public double RelativeEffectiveDepthSectionLengthMaxValue
        {
            get => relativeEffectiveDepthSectionLengthMaxValue;
            set
            {
                if (value <= 0)
                {
                    throw new StructureHelperException($"Maximum inclined section factor must be greater than zero, but was {value}");
                }
                relativeEffectiveDepthSectionLengthMaxValue = value;
            }
        }

        public double RelativeEffectiveDepthSectionLengthMinValue
        {
            get => relativeEffectiveDepthSectionLengthMinValue;
            set
            {
                if (value < 0)
                {
                    throw new StructureHelperException($"Maximum inclined section factor must be greater than zero, but was {value}");
                }
                relativeEffectiveDepthSectionLengthMinValue = value;
            }
        }

        public BeamShearDesignRangeProperty(Guid id)
        {
            Id = id;
        }

        /// <inheritdoc>
        public object Clone()
        {
            var updateStrategy = new BeamShearDesignRangePropertyUpdateStrategy();
            BeamShearDesignRangeProperty newitem = new(Guid.NewGuid());
            updateStrategy.Update(newitem, this);
            return newitem;
        }
    }
}
