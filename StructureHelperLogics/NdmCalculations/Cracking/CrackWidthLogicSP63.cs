using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthLogicSP63 : ICrackWidthLogic
    {
        CrackWidthLogicInputDataSP63 inputData;
        public ICrackWidthLogicInputData InputData {get;set;}

        public double GetCrackWidth()
        {
            CheckOptions();
            //check if strain of concrete greater than strain of rebar
            if (inputData.ConcreteStrain > inputData.RebarStrain) { return 0d; }
            double width = (inputData.RebarStrain - inputData.ConcreteStrain) * inputData.Length;
            width *= inputData.TermFactor * inputData.BondFactor * inputData.StressStateFactor * inputData.PsiSFactor;
            return width;
        }

        private void CheckOptions()
        {
            if (InputData is not CrackWidthLogicInputDataSP63)
            {
                throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(CrackWidthLogicInputDataSP63), InputData.GetType()));
            }
            inputData = InputData as CrackWidthLogicInputDataSP63;
            if (inputData.Length <=0d)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": length between cracks L={inputData.Length} must be greate than zero");
            }
            if (inputData.TermFactor <= 0d)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Term factor {inputData.TermFactor} must be greate than zero");
            }
            if (inputData.BondFactor <= 0d)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Bond factor {inputData.BondFactor} must be greate than zero");
            }
            if (inputData.StressStateFactor <= 0d)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Bond factor {inputData.StressStateFactor} must be greate than zero");
            }
            if (inputData.PsiSFactor <= 0d)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": PsiS factor {inputData.PsiSFactor} must be greate than zero");
            }
        }
    }
}
