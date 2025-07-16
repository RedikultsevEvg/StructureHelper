using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services
{
    public class InterpolateValueLogic : IInterpolateValueLogic
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double KnownValueX { get; set; }

        public double GetValueY()
        {
            Check();
            double tangent = (Y2-Y1) / (X2-X1);
            double value = Y1 + (tangent * (KnownValueX - X1));
            return value;
        }

        private void Check()
        {
            if (X1 == X2)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Value X1 = {X1} must be inequal to value X2, but it was");
            }
        }
    }
}
