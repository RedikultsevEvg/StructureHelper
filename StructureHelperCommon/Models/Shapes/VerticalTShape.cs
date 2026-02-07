using StructureHelperCommon.Infrastructures.Exceptions;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc/>
    public class VerticalTShape : IVerticalTShape
    {
        private const double delta = 1e-10;
        private const string flangeHeightString = "Height of flange";
        private const string flangeWidthString = "Width of flange";
        private const string greaterThanZeroString = "must be greater than zero";
        private double fullHeight = 0.6; //m
        private double webWidth = 0.2; //m
        private double flangeHeight = 0.1; //m
        private double flangeWidth = 0.4; //m
        private double flangeXOffset = 0.0; //m

        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public double FullHeight
        {
            get => fullHeight;
            set
            {
                if (value <= 0)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Full height of T-shape {greaterThanZeroString}");
                }
                fullHeight = value;
            }
        }
        /// <inheritdoc/>
        public double WebWidth
        {
            get => webWidth;
            set
            {
                if (value <= 0)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Width of web of T-shape {greaterThanZeroString}");
                }
                webWidth = value;
            }
        }
        /// <inheritdoc/>
        public double FlangeHeight
        {
            get => flangeHeight;
            set
            {
                if (value <= 0)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": {flangeHeightString} of T-shape {greaterThanZeroString}, but was {value}");
                }
                if (value > fullHeight - delta)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": {flangeHeightString} must be less than full height, but was {value}");
                }
                flangeHeight = value;
            }
        }
        /// <inheritdoc/>
        public double FlangeWidth
        {
            get => flangeWidth;
            set
            {
                if (value <= 0)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": {flangeWidthString} of T-shape {greaterThanZeroString}, but was {value}");
                }
                if (value < webWidth + delta)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": {flangeWidthString} of T-shape must be greater than width of web, but was {value}");
                }
                flangeWidth = value;
            }
        }
        /// <inheritdoc/>
        public double FlangeXOffset
        {
            get => flangeXOffset;
            set
            {
                flangeXOffset = value;
            }
        }

        public VerticalTShape() : this(Guid.NewGuid())
        {
            
        }
        public VerticalTShape(Guid id)
        {
            Id = id;
        }
    }
}
