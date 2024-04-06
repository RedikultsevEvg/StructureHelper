using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthSimpleCalculator : ICalculator
    {
        ICrackWidthLogic crackWidthLogic = new CrackWidthLogicSP63();
        CrackWidthSimpleCalculatorResult result;
        public string Name { get; set; }
        public ICrackWidthSimpleCalculatorInputData InputData { get; set; }
        public IResult Result => result;

        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public void Run()
        {
            result = new() { IsValid = true};
            var crackWidthLogicType = CrackWidthLogicType.SP63;
            var logicInputData = CrackWidthLogicInputDataFactory.GetCrackWidthLogicInputData(crackWidthLogicType, InputData);
            crackWidthLogic.InputData = logicInputData;
            double crackWidth = 0d;
            try
            {
                crackWidth = crackWidthLogic.GetCrackWidth();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description += "\n" + ex;
            }
            result.RebarPrimitive = InputData.RebarPrimitive;
            //result.CrackWidth = crackWidth;
            //result.RebarStrain = logicInputData.RebarStrain;
            //result.ConcreteStrain = logicInputData.ConcreteStrain;
        }
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
