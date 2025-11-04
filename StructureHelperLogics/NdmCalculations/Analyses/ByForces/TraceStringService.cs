using LoaderCalculator.Data.Matrix;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public static class TraceStringService
    {
        public static string GetLimitStateAndCalctTerm(LimitStates limitState, CalcTerms calcTerm)
        {
            string s = "Limit state: " + limitState.ToString();
            s += ", Calculation term: " + calcTerm.ToString();
            return s;
        }

        public static string GetLimitStateAndCalctTerm(IDesignForceTuple designForceTuple)
        {
            return GetLimitStateAndCalctTerm(designForceTuple.LimitState, designForceTuple.CalcTerm);
        }

        public static string GetLimitStateAndCalctTerm(IStateCalcTermPair stateCalcTermPair)
        {
            return GetLimitStateAndCalctTerm(stateCalcTermPair.LimitState, stateCalcTermPair.CalcTerm);
        }

        public static string GetForces(IForceTuple forceTuple)
        {
            string s = "Mx = " + forceTuple.Mx.ToString();
            s += "(N*m), My = " + forceTuple.My.ToString();
            s += "(N*m), Nz =" + forceTuple.Nz.ToString() + "(N), ";
            return s;
        }
        public static string GetCuvatures(IStrainMatrix strainMatrix)
        {
            string s = "Kx = " + strainMatrix.Kx;
            s += "(1/m), Ky = " + strainMatrix.Ky;
            s += "(1/m), EpsZ = " + strainMatrix.EpsZ + "(dimensionless)";
            return s;
        }
    }
}
