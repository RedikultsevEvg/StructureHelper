using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class GetInclinedSectionLogic : IGetInclinedSectionLogic
    {
        private readonly IBeamShearSection beamShearSection;
        private readonly double startCoord;
        private readonly double endCoord;
        private double width;
        private double depth;
        private double effectiveDepth;

        public GetInclinedSectionLogic(
            IBeamShearSection beamShearSection,
            double startCoord,
            double endCoord,
            IShiftTraceLogger? traceLogger)
        {
            this.beamShearSection = beamShearSection;
            this.startCoord = startCoord;
            this.endCoord = endCoord;
            TraceLogger = traceLogger;
            Check();
        }

        public GetInclinedSectionLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public IInclinedSection GetInclinedSection()
        {
            if (beamShearSection.Shape is IRectangleShape rectangle)
            {
                width = rectangle.Width;
                depth = rectangle.Height;
            }
            else if (beamShearSection.Shape is ICircleShape circle)
            {
                width = circle.Diameter;
                depth = circle.Diameter;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(beamShearSection.Shape));
            }
            effectiveDepth = depth - beamShearSection.CenterCover;
            InclinedSection inclinedSection = new()
            {
                EffectiveDepth = effectiveDepth,
                StartCoord = startCoord,
                EndCoord = endCoord,
                WebWidth = width
            };
            return inclinedSection;
        }

        private void Check()
        {
            if (beamShearSection is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": Beam shear section";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            if (beamShearSection.Shape is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": Beam section shape";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }
    }
}
