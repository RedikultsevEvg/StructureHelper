using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelperCommon.Models.Loggers;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    public class ProcessEccentricity : IProcessorLogic<IForceTuple>
    {
        private IProcessorLogic<IForceTuple> eccentricityLogic;

        public ICompressedMember CompressedMember { get; private set; }
        public IEnumerable<INdm> Ndms { get; private set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public IForceTuple InputForceTuple { get; set; }

        private double sizeX;
        private double sizeY;

        public ProcessEccentricity(IProcessorLogic<IForceTuple> eccentricityLogic)
        {
            this.eccentricityLogic = eccentricityLogic;
        }
        public ProcessEccentricity(ICompressedMember compressedMember, IEnumerable<INdm> ndms, IForceTuple initialForceTuple)
        {
            CompressedMember = compressedMember;
            Ndms = ndms;
            InputForceTuple = initialForceTuple;
            sizeX = ndms.Max(x => x.CenterX) - ndms.Min(x => x.CenterX);
            sizeY = ndms.Max(x => x.CenterY) - ndms.Min(x => x.CenterY);
            eccentricityLogic = new RcEccentricityLogic()
            {
                InputForceTuple = InputForceTuple,
                Length = CompressedMember.GeometryLength,
                SizeX = sizeX,
                SizeY = sizeY,
            };
        }

        public IForceTuple GetValue()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage("Get eccentricity taking into account accidental eccentricity");
            TraceLogger?.AddMessage(string.Format("Cross-section size along x-axis dx = {0}, along y-axis dy = {1}", sizeX, sizeY));

            eccentricityLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);          
            var newTuple = eccentricityLogic.GetValue();
            return newTuple;
        }
    }
}
