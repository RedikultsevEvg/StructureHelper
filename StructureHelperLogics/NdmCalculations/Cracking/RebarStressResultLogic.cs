using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarStressResultLogic : IRebarStressResultLogic
    {
        private IRebarStressCalculator rebarStressCalculator;
        public IRebarNdmPrimitive RebarPrimitive { get; set; }
        public IRebarCrackInputData RebarCrackInputData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public RebarStressResultLogic(IRebarStressCalculator rebarStressCalculator, IShiftTraceLogger? traceLogger)
        {
            this.rebarStressCalculator = rebarStressCalculator;
            this.TraceLogger = traceLogger;
        }

        public RebarStressResultLogic() : this(new RebarStressCalculator(), null)
        {

        }

        public IRebarStressResult GetRebarStressResult()
        {
            IRebarStressCalculatorInputData calculatorInputData = new RebarStressCalculatorInputData()
            {
                ForceTuple = RebarCrackInputData.ForceTuple,
                NdmCollection = RebarCrackInputData.CrackedNdmCollection,
                RebarPrimitive = RebarPrimitive,
            };
            rebarStressCalculator.InputData = calculatorInputData;
            rebarStressCalculator.Run();
            var result = rebarStressCalculator.Result as RebarStressResult;
            if (result.IsValid == false)
            {
                string errorString = LoggerStrings.CalculationError + result.Description;
                TraceLogger?.AddMessage($"Rebar name: {RebarPrimitive.Name}\n" + errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            return result;
        }
    }
}
