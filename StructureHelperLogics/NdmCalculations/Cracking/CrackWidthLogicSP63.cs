using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
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
        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetCrackWidth()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage("Method of crack width calculation based on SP 63.13330.2018");
            CheckOptions();
            TraceLogger?.AddMessage($"Term factor fi1 = {inputData.TermFactor}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Bond factor fi2 = {inputData.BondFactor}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Stress state factor fi3 = {inputData.StressStateFactor}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"PsiS factor PsiS = {inputData.PsiSFactor}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Length between cracks Ls = {inputData.Length}", TraceLogStatuses.Service);
            //check if strain of concrete greater than strain of rebar
            double rebarElongation = inputData.RebarStrain - inputData.ConcreteStrain;
            if (rebarElongation < 0d)
            {
                TraceLogger?.AddMessage($"Elongation of rebar is negative, may be rebar is under compression, width of crack a,crc = 0");
                return 0d;
            }
            TraceLogger?.AddMessage($"Rebar elongation Epsilon = {inputData.RebarStrain} - {inputData.ConcreteStrain} = {rebarElongation}(dimensionless)");
            double width = rebarElongation * inputData.Length;
            width *= inputData.TermFactor * inputData.BondFactor * inputData.StressStateFactor * inputData.PsiSFactor;
            TraceLogger?.AddMessage($"Width of crack a,crc = {inputData.TermFactor} * {inputData.BondFactor} * {inputData.StressStateFactor} * {inputData.PsiSFactor} * {rebarElongation} * {inputData.Length}(m) = {width}(m)");
            return width;
        }

        private void CheckOptions()
        {
            string errorString = string.Empty;
            if (InputData is not CrackWidthLogicInputDataSP63)
            {
                errorString = ErrorStrings.ExpectedWas(typeof(CrackWidthLogicInputDataSP63), InputData.GetType());
            }
            inputData = InputData as CrackWidthLogicInputDataSP63;
            if (inputData.Length <=0d)
            {
                errorString = ErrorStrings.DataIsInCorrect + $": length between cracks Lcrc={inputData.Length} must be greater than zero";
            }
            if (inputData.TermFactor <= 0d)
            {
                errorString = ErrorStrings.DataIsInCorrect + $": Term factor fi1 {inputData.TermFactor} must be greater than zero";

            }
            if (inputData.BondFactor <= 0d)
            {
                errorString = ErrorStrings.DataIsInCorrect + $": Bond factor fi2 {inputData.BondFactor} must be greater than zero";

            }
            if (inputData.StressStateFactor <= 0d)
            {
                errorString = ErrorStrings.DataIsInCorrect + $": Stress factor fi3 factor {inputData.StressStateFactor} must be greater than zero";

            }
            if (inputData.PsiSFactor <= 0d)
            {
                 errorString = ErrorStrings.DataIsInCorrect + $": PsiS factor {inputData.PsiSFactor} must be greater than zero";
            }
            if (errorString != string.Empty)
            {
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            TraceLogger?.AddMessage($"Checking parameters has done succefully", TraceLogStatuses.Service);
        }
    }
}
