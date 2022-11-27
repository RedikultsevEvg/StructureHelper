using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class DesignForceTuple : IDesignForceTuple
    {
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public IForceTuple ForceTuple { get; private set; }

        public DesignForceTuple(LimitStates limitState, CalcTerms calcTerm)
        {
            LimitState = limitState;
            CalcTerm = calcTerm;
            ForceTuple = new ForceTuple();
        }
    }
}
