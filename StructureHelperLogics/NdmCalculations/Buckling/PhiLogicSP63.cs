using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    public class PhiLogicSP63 : IConcretePhiLLogic
    {
        private const double maxValueOfPhiL = 2d;
        private const double minValueOfPhiL = 1d;
        readonly IForceTuple fullForceTuple;
        readonly IForceTuple longForceTuple;
        readonly IPoint2D point;
        public PhiLogicSP63(IForceTuple fullForceTuple, IForceTuple longForceTuple, IPoint2D point)
        {
            this.fullForceTuple = fullForceTuple;
            this.longForceTuple = longForceTuple;
            this.point = point;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetPhil()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            var distance = Math.Sqrt(point.X * point.X + point.Y * point.Y);
            string distMessage = string.Format("Distance = Sqrt(dX ^2 + dY^2) = Sqrt(({0})^2 + ({1})^2) = {2}, m", point.X, point.Y, distance);
            TraceLogger?.AddMessage(distMessage);
            var fullMoment = GetMoment(fullForceTuple, distance);
            string fullMomentMessage = string.Format("FullMoment = {0}, N*m", fullMoment);
            TraceLogger?.AddMessage(fullMomentMessage);
            var longMoment = GetMoment(longForceTuple, distance);
            string longMomentMessage = string.Format("LongMoment = {0}, N*m", longMoment);
            TraceLogger?.AddMessage(longMomentMessage);
            if (fullMoment == 0d)
            {
                return maxValueOfPhiL;
            }
            var phiL = 1 + longMoment / fullMoment;
            string phiLMessage = string.Format("PhiL = 1 + LongMoment / FullMoment = 1+ {0} / {1} = {2}, (dimensionless)", longMoment, fullMoment, phiL);
            TraceLogger?.AddMessage(phiLMessage);
            TraceLogger?.AddMessage(string.Format("But not less than {0}, and not greater than {1}", minValueOfPhiL, maxValueOfPhiL));
            phiL = Math.Max(minValueOfPhiL, phiL);
            phiL = Math.Min(maxValueOfPhiL, phiL);
            TraceLogger?.AddMessage(string.Format("PhiL =  {0}, (dimensionless)", phiL));
            return phiL;
        }

        private double GetMoment(IForceTuple forceTuple, double distance)
        {
            double nz = Math.Abs(forceTuple.Nz);
            double mx = Math.Abs(forceTuple.Mx);
            double my = Math.Abs(forceTuple.My);
            double moment = nz * distance + mx + my;
            string message = string.Format("Moment = Nz * distance + Abs(Mx) + Abs(My) = {0} * {1} + {2} + {3} = {4}, N *m", nz, distance, mx, my, moment);
            TraceLogger?.AddMessage(message);
            return moment;
        }
    }
}
