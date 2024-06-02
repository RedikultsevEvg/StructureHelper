using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Sections.Logics
{
    public class RcAccEccentricityLogic : IRcAccEccentricityLogic
    {
        private const string accEccMessage = "Accidental eccentricity along {0}-axis";
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
        public IShiftTraceLogger? TraceLogger { get; set; }
        private IRcEccentricityAxisLogic eccentricityLogic;
        public RcAccEccentricityLogic(IRcEccentricityAxisLogic eccentricityLogic)
        {
            this.eccentricityLogic = eccentricityLogic;
        }
        public RcAccEccentricityLogic() : this(new RcEccentricityAxisLogic())
        {
                
        }
        public (double ex, double ey) GetValue()
        {
            eccentricityLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage(string.Format(accEccMessage, "x"));
            eccentricityLogic.Length = Length;
            eccentricityLogic.Size = SizeX;
            var xFullEccentricity = eccentricityLogic.GetValue();
            TraceLogger?.AddMessage(string.Format(accEccMessage, "y"));
            eccentricityLogic.Size = SizeY;
            var yFullEccentricity = eccentricityLogic.GetValue();
            return (xFullEccentricity, yFullEccentricity);
        }
    }
}
