using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Sections.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Sections
{
    public class RcEccentricityLogic : IProcessorLogic<IForceTuple>, IHasInputForce
    {
        private const string fstAxisName = "x";
        private const string sndAxisName = "y";
        private const string actualEccMessage = "Actual eccentricity e0,{0} = {1}(m)";
        private const string maxEccentricityMessage = "Eccentricity e,{0} = max({1}(m); {2}(m)) = {3}(m)";
        private const string OutPutBendingMomentMessage = "Bending moment arbitrary {0}-axis M{0} = Nz * e,{0} = {1}(N) * {2}(m) = {3}(N*m)";

        /// <summary>
        /// Properties of compressed member
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Size of cross-section along X-axis, m
        /// </summary>
        public double SizeX { get; set; }
        /// <summary>
        /// Size of cross-section along Y-axis, m
        /// </summary>
        public double SizeY { get; set; }
        ///<inheritdoc/>
        public IForceTuple? InputForceTuple { get; set; }
        ///<inheritdoc/>
        public IShiftTraceLogger? TraceLogger { get; set; }
        public IRcAccEccentricityLogic EccentricityLogic { get; private set; }

        public RcEccentricityLogic(IRcAccEccentricityLogic eccentricityLogic)
        {
            EccentricityLogic = eccentricityLogic;
        }

        public RcEccentricityLogic() : this(new RcAccEccentricityLogic())
        {
            
        }

        public IForceTuple GetValue()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            if (InputForceTuple is null)
            {
                string errorString = ErrorStrings.NullReference + $": {nameof(InputForceTuple)}";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }

            EccentricityLogic.Length = Length;
            EccentricityLogic.SizeX = SizeX;
            EccentricityLogic.SizeY = SizeY;
            EccentricityLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);

            var (ex, ey) = EccentricityLogic.GetValue();

            var xEccentricity = Math.Abs(InputForceTuple.My / InputForceTuple.Nz);
            TraceLogger?.AddMessage(string.Format(actualEccMessage, fstAxisName, xEccentricity));
            var yEccentricity = Math.Abs(InputForceTuple.Mx / InputForceTuple.Nz);
            TraceLogger?.AddMessage(string.Format(actualEccMessage, sndAxisName, yEccentricity));

            var xFullEccentricity = Math.Max(ex, xEccentricity);
            var yFullEccentricity = Math.Max(ey, yEccentricity);
            string mesEx = string.Format(maxEccentricityMessage, fstAxisName, ex, xEccentricity, xFullEccentricity);
            TraceLogger?.AddMessage(mesEx);
            string mesEy = string.Format(maxEccentricityMessage, sndAxisName, ey, yEccentricity, yFullEccentricity);
            TraceLogger?.AddMessage(mesEy);
            var xSign = InputForceTuple.Mx == 0d ? -1d : Math.Sign(InputForceTuple.Mx);
            var ySign = InputForceTuple.My == 0d ? -1d : Math.Sign(InputForceTuple.My);
            var mx = (-1d) * InputForceTuple.Nz * yFullEccentricity * xSign;
            var my = (-1d) * InputForceTuple.Nz * xFullEccentricity * ySign;
            TraceLogger?.AddMessage(string.Format(OutPutBendingMomentMessage, fstAxisName, InputForceTuple.Nz, yFullEccentricity, mx), TraceLogStatuses.Debug);
            TraceLogger?.AddMessage(string.Format(OutPutBendingMomentMessage, sndAxisName, InputForceTuple.Nz, xFullEccentricity, my), TraceLogStatuses.Debug);

            var newTuple = new ForceTuple()
            {
                Mx = mx,
                My = my,
                Nz = InputForceTuple.Nz,
                Qx = InputForceTuple.Qx,
                Qy = InputForceTuple.Qy,
                Mz = InputForceTuple.Mz,
            };
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(newTuple));
            return newTuple;
        }
    }
}
