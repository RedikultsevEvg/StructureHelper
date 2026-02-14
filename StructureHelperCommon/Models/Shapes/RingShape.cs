using StructureHelperCommon.Infrastructures.Exceptions;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class RingShape : IRingShape
    {
        private const double minValue = 1e-10;
        private double outerDiameter;
        private double innerDiameter;

        public double OuterDiameter
        {
            get => outerDiameter;
            set
            {
                if (value <= minValue)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Outer Diameter of o-shape must be greater than minimum value {minValue}, but was {value}");
                }
                outerDiameter = value;
            }
        }
        public double InnerDiameter
        {
            get => innerDiameter;
            set
            {
                if (value <= minValue)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Inner Diameter of o-shape must be greater than minimum value {minValue}, but was {value}");
                }
                innerDiameter = value;
            }
        }
        public double OuterRadius
        {
            get
            {
                return outerDiameter / 2.0;
            }

            set
            {
                if (value <= minValue)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Outer Radius of o-shape must be greater than minimum value {minValue}, but was {value}");
                }
                outerDiameter = 2.0 * value;
            }
        }
        public double InnerRadius
        {
            get
            {
                return innerDiameter / 2.0;
            }

            set
            {
                if (value <= minValue)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Inner Radius of o-shape must be greater than minimum value {minValue}, but was {value}");
                }
                innerDiameter = 2.0 * value;
            }
        }

        public Guid Id { get; }

        public RingShape() : this(Guid.NewGuid()) { }

        public RingShape(Guid id)
        {
            Id = id;
        }
    }
}
