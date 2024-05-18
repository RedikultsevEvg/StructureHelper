using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoaderCalculator.Logics;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarStressCalculator : ICalculator
    {
        private IStressLogic stressLogic;
        private Ndm concreteNdm;
        private RebarNdm rebarNdm;
        private RebarStressResult result;

        public ForceTuple ForceTuple { get; set; }
        public IEnumerable<INdm> NdmCollection { get; set; }
        public RebarPrimitive RebarPrimitive { get; set; }
        public string Name { get; set; }

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public StrainTuple GetStrainTuple()
        {
            IForceTupleInputData inputData = new ForceTupleInputData()
            {
                NdmCollection = NdmCollection,
                Tuple = ForceTuple
            };
            IForceTupleCalculator calculator = new ForceTupleCalculator()
            {
                InputData = inputData,
                //TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            calculator.Run();
            var forceResult = calculator.Result as IForcesTupleResult;
            if (forceResult.IsValid == false)
            {
                //TraceLogger?.AddMessage(LoggerStrings.CalculationError + $": {forceResult.Description}", TraceLogStatuses.Error);
                throw new StructureHelperException(ErrorStrings.CalculationError);
            }
            var strain = TupleConverter.ConvertToStrainTuple(forceResult.LoaderResults.StrainMatrix);
            return strain;
        }
        public RebarStressCalculator(IStressLogic stressLogic)
        {
            this.stressLogic = stressLogic;
        }
        public RebarStressCalculator() : this(new StressLogic())
        {
            
        }


        public void Run()
        {
            GetNdms();
            result = new RebarStressResult()
            {
                IsValid = true,
                Description = string.Empty
            };
            var strainTuple = GetStrainTuple();
            result.StrainTuple = strainTuple;
            var strainMatrix = TupleConverter.ConvertToLoaderStrainMatrix(strainTuple);
            result.RebarStrain = stressLogic.GetTotalStrain(strainMatrix, rebarNdm);
            result.RebarStress = stressLogic.GetStress(strainMatrix, rebarNdm);
            result.ConcreteStrain = concreteNdm.Prestrain;
        }


        private void GetNdms()
        {
            var options = new TriangulationOptions()
            {
                CalcTerm = CalcTerms.ShortTerm,
                LimiteState = LimitStates.SLS,
            };
            concreteNdm = RebarPrimitive.GetConcreteNdm(options);
            concreteNdm.StressScale = 1d;
            rebarNdm = RebarPrimitive.GetRebarNdm(options);
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
