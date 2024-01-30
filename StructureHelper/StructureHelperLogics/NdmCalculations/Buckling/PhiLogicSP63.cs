using StructureHelperCommon.Models.Forces;
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
        readonly IForceTuple fullForceTuple;
        readonly IForceTuple longForceTuple;
        readonly IPoint2D point;
        public PhiLogicSP63(IForceTuple fullForceTuple, IForceTuple longForceTuple, IPoint2D point)
        {
            this.fullForceTuple = fullForceTuple;
            this.longForceTuple = longForceTuple;
            this.point = point;
        }

        public double GetPhil()
        {
            var distance = Math.Sqrt(point.X * point.X + point.Y * point.Y);
            var fullMoment = GetMoment(fullForceTuple, distance);
            var longMoment = GetMoment(longForceTuple, distance);
            if (fullMoment == 0d) { return 2d; }
            var phi = 1 + longMoment / fullMoment;
            phi = Math.Max(1, phi);
            phi = Math.Min(2, phi);
            return phi;
        }

        private double GetMoment(IForceTuple forceTuple, double distance)
        {
            return Math.Abs(forceTuple.Nz) * distance + Math.Abs(forceTuple.Mx) + Math.Abs(forceTuple.My);
        }
    }
}
