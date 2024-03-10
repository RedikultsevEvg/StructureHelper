using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Sections.Logics
{
    public class RcEccentricityAxisLogic : IRcEccentricityAxisLogic
    {
        private readonly double lengthFactor;
        private readonly double sizeFactor;
        private readonly double minEccentricity;

        /// <summary>
        /// Properties of compressed member
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Size of cross-section along X-axis, m
        /// </summary>
        public double Size { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public RcEccentricityAxisLogic()
        {
            lengthFactor = 600d;
            sizeFactor = 30d;
            minEccentricity = 0.01d;
        }
        public double GetValue()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            var lengthEccetricity = Length / lengthFactor;
            TraceLogger?.AddMessage(string.Format("Length of member = {0}(m)", Length));
            TraceLogger?.AddMessage(string.Format("Accidental eccentricity by length e,a = {0}(m) / {1} = {2}(m)", Length, lengthFactor, lengthEccetricity));
            TraceLogger?.AddMessage(string.Format("Size of cross-section of member = {0}(m)", Size));
            var sizeXEccetricity = Size / sizeFactor;
            TraceLogger?.AddMessage(string.Format("Accidental eccentricity by size e,a ={0}(m) / {1} = {2}(m)", Size, sizeFactor, sizeXEccetricity)); ;
            TraceLogger?.AddMessage(string.Format("In any case,  minimum accidental eccentricity e,a = {0}(m)", minEccentricity));

            var fullEccentricity = new List<double>()
            {
                lengthEccetricity,
                sizeXEccetricity,
                minEccentricity,
            }
            .Max();
            string mesEcc = string.Format("Maximum accidental eccentricity e,a = max({0}(m); {1}(m); {2}(m)) = {3}(m)",
                lengthEccetricity, sizeXEccetricity,
                minEccentricity,
                fullEccentricity);
            TraceLogger?.AddMessage(mesEcc);
            return fullEccentricity;
        }
    }
}
