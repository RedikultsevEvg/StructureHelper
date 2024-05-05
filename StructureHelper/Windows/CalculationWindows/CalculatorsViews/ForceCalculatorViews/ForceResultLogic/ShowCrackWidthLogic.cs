using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    internal class ShowCrackWidthLogic
    {
        public List<INdmPrimitive> ndmPrimitives { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public IForceTuple ForceTuple { get; set; }

        internal void Show()
        {
            var inputData = new TupleCrackInputData()
            {
                //LimitState = LimitState,
                //CalcTerm = CalcTerm,
                LongTermTuple = ForceTuple,
                NdmPrimitives = ndmPrimitives
            };
            var calculator = new TupleCrackCalculator() { InputData = inputData };
            calculator.Run();
            var result = calculator.Result;

        }
    }
}
