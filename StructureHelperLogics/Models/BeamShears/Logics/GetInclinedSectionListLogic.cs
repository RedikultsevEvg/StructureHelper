using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class GetInclinedSectionListLogic : IGetInclinedSectionListLogic
    {
        private readonly IGetInclinedSectionListInputData inputData;
        private IGetInclinedSectionLogic inclinedSectionLogic;
        private double depth;
        private double effectiveDepth;
        private List<IInclinedSection> inclinedSections;
        private List<double> coordinates;
        public IShiftTraceLogger? TraceLogger { get; set; }

        public GetInclinedSectionListLogic(
            IGetInclinedSectionListInputData inputData,
            IShiftTraceLogger? traceLogger)
        {
            this.inputData = inputData;
            TraceLogger = traceLogger;
        }


        public List<IInclinedSection> GetInclinedSections()
        {
            Check();
            GetShapeParameters();
            GetCoordinates();
            foreach (var startCoord in coordinates)
            {
                var endCoordinates = coordinates.Where(x => x >= startCoord);
                foreach (var endCoord in endCoordinates)
                {
                    inclinedSectionLogic = InitializeInclinedSectionLogic(startCoord, endCoord);
                    IInclinedSection inclinedSection = inclinedSectionLogic.GetInclinedSection();
                    inclinedSections.Add(inclinedSection);
                }
            }
            return inclinedSections;
        }

        private void GetCoordinates()
        {
            double maxSectionLength = GetMaxSectionLength();
            int stepCount = inputData.DesignRangeProperty.StepCount;
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

        private double GetMaxSectionLength()
        {
            double relativeLength = inputData.DesignRangeProperty.RelativeEffectiveDepthRangeValue * effectiveDepth;
            double length = Math.Max(relativeLength, inputData.DesignRangeProperty.AbsoluteRangeValue);
            return length;
        }

        private void Check()
        {
            CheckObject.IsNull(inputData);
            CheckObject.IsNull(inputData.BeamShearSection);
        }
        private IGetInclinedSectionLogic InitializeInclinedSectionLogic(double startCoord, double endCoord)
        {
            if (inputData.GetInclinedSectionLogic is not null)
            {
                return inputData.GetInclinedSectionLogic;
            }
            return new GetInclinedSectionLogic(inputData.BeamShearSection, startCoord, endCoord, TraceLogger);
        }
        private void GetShapeParameters()
        {
            if (inputData.BeamShearSection.Shape is IRectangleShape rectangle)
            {
                depth = rectangle.Height;
            }
            else if (inputData.BeamShearSection.Shape is ICircleShape circle)
            {
                depth = circle.Diameter;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(inputData.BeamShearSection.Shape));
            }
            effectiveDepth = depth - inputData.BeamShearSection.CenterCover;
        }
    }
}
