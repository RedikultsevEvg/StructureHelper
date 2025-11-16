using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class GetInclinedSectionListLogic : IGetInclinedSectionListLogic
    {
        private const int minimumInclinedSectionLengthFactor = 0;
        private const int maximumInclinedSectionLengthFactor = 3;
        private IGetInclinedSectionLogic inclinedSectionLogic;
        private double depth;
        private double effectiveDepth;
        private List<IInclinedSection> inclinedSections;
        private List<double> coordinates;

        public IBeamShearDesignRangeProperty DesignRangeProperty { get; set; }
        public IBeamShearSection BeamShearSection { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public GetInclinedSectionListLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }


        public List<IInclinedSection> GetInclinedSections()
        {
            Check();
            GetShapeParameters();
            GetCoordinates();
            double minSectionLength = minimumInclinedSectionLengthFactor * effectiveDepth;
            double maxSectionLength = maximumInclinedSectionLengthFactor * effectiveDepth;
            foreach (var startCoord in coordinates)
            {
                var endCoordinates = coordinates.Where(x => x >= startCoord);
                foreach (var endCoord in endCoordinates)
                {
                    double sectionLength = endCoord - startCoord;
                    if (sectionLength >= minSectionLength & sectionLength <= maxSectionLength)
                    {
                        inclinedSectionLogic = new GetInclinedSectionLogic(BeamShearSection, startCoord, endCoord, TraceLogger);
                        IInclinedSection inclinedSection = inclinedSectionLogic.GetInclinedSection();
                        inclinedSections.Add(inclinedSection);
                    }  
                }
            }
            return inclinedSections;
        }

        private void GetCoordinates()
        {
            double maxSectionLength = GetMaxLengthOfInclinedSection();
            int stepCount = DesignRangeProperty.StepCount;
            double step = maxSectionLength / stepCount;
            inclinedSections = new();
            coordinates = new();
            for (int i = 0; i < stepCount + 1; i++)
            {
                double endCoord = step * i;
                double roundedEndCoord = Math.Round(endCoord, 6);
                coordinates.Add(roundedEndCoord);
            }
        }

        private double GetMaxLengthOfInclinedSection()
        {
            double minimumRelativeLength = DesignRangeProperty.RelativeEffectiveDepthRangeValue * effectiveDepth;
            double minimumAbsoluteLength = DesignRangeProperty.AbsoluteRangeValue;
            double length = Math.Max(minimumRelativeLength, minimumAbsoluteLength);
            return length;
        }

        private void Check()
        {
            CheckObject.ThrowIfNull(BeamShearSection);
            CheckObject.ThrowIfNull(DesignRangeProperty);
        }

        private void GetShapeParameters()
        {
            if (BeamShearSection.Shape is IRectangleShape rectangle)
            {
                depth = rectangle.Height;
            }
            else if (BeamShearSection.Shape is ICircleShape circle)
            {
                depth = circle.Diameter;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(BeamShearSection.Shape));
            }
            double coverLayerOfConcrete = BeamShearSection.CenterCover;
            effectiveDepth = depth - coverLayerOfConcrete;
        }
    }
}
