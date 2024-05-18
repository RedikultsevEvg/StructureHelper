using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class StabSoftetingLogic : ICrackSofteningLogic
    {
        private double stabSofteningValue;
        public IShiftTraceLogger? TraceLogger { get; set; }
        public StabSoftetingLogic(double stabSofteningValue)
        {
            this.stabSofteningValue = stabSofteningValue;
        }
        public double GetSofteningFactor()
        {
            TraceLogger?.AddMessage($"Constant value of softening factor PsiS = {stabSofteningValue}");
            return stabSofteningValue;
        }
    }
}
