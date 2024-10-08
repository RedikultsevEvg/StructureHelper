using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthLogicSP63 : ICrackWidthLogic
    {
        CrackWidthLogicInputDataSP63 inputData;
        private double widthOfCrack;
        private ICheckInputDataLogic<CrackWidthLogicInputDataSP63> checkInputDataLogic;

        public ICrackWidthLogicInputData InputData {get;set;}
        public IShiftTraceLogger? TraceLogger { get; set; }

        public CrackWidthLogicSP63(ICheckInputDataLogic<CrackWidthLogicInputDataSP63> checkInputDataLogic, IShiftTraceLogger? traceLogger)
        {
            this.checkInputDataLogic = checkInputDataLogic;
            this.TraceLogger = traceLogger;
        }

        public CrackWidthLogicSP63() : this (new CheckCrackWidthSP63InputDataLogic(), null)
        {
            
        }

        public double GetCrackWidth()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            CheckOptions();
            TraceLogger?.AddMessage("Method of crack width calculation based on SP 63.13330.2018");
            TraceInputData();
            //check if strain of concrete greater than strain of rebar
            CalculateWidthOfCrack();
            return widthOfCrack;
        }

        private void CalculateWidthOfCrack()
        {
            double rebarElongation = inputData.RebarStrain - inputData.ConcreteStrain;
            if (rebarElongation < 0d)
            {
                TraceLogger?.AddMessage($"Elongation of rebar is negative, may be rebar is under compression, width of crack a,crc = 0");
                widthOfCrack = 0d;
            }
            else
            {
                TraceLogger?.AddMessage($"Rebar elongation Epsilon = {inputData.RebarStrain} - ({inputData.ConcreteStrain}) = {rebarElongation}(dimensionless)");
                widthOfCrack = rebarElongation * inputData.LengthBetweenCracks;
                widthOfCrack *= inputData.TermFactor * inputData.BondFactor * inputData.StressStateFactor * inputData.PsiSFactor;
                TraceLogger?.AddMessage($"Width of crack a,crc = {inputData.TermFactor} * {inputData.BondFactor} * {inputData.StressStateFactor} * {inputData.PsiSFactor} * {rebarElongation} * {inputData.LengthBetweenCracks}(m) = {widthOfCrack}(m)");
            }
        }

        private void TraceInputData()
        {
            TraceLogger?.AddMessage($"Term factor fi1 = {inputData.TermFactor}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Bond factor fi2 = {inputData.BondFactor}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Stress state factor fi3 = {inputData.StressStateFactor}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"PsiS factor PsiS = {inputData.PsiSFactor}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Length between cracks Ls = {inputData.LengthBetweenCracks}", TraceLogStatuses.Service);
        }

        private bool CheckOptions()
        {
            string errorString = string.Empty;
            if (InputData is not CrackWidthLogicInputDataSP63)
            {
                errorString = ErrorStrings.ExpectedWas(typeof(CrackWidthLogicInputDataSP63), InputData.GetType());
                throw new StructureHelperException(errorString);
            }
            inputData = InputData as CrackWidthLogicInputDataSP63;
            checkInputDataLogic.InputData = inputData;
            checkInputDataLogic.TraceLogger = TraceLogger;
            if (checkInputDataLogic.Check() != true)
            {
                throw new StructureHelperException(errorString);
                return false;
            }
            TraceLogger?.AddMessage($"Checking parameters has done succefully", TraceLogStatuses.Service);
            return true;
        }
    }
}
