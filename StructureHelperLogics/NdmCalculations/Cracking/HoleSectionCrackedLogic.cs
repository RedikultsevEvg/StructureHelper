using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    internal class HoleSectionCrackedLogic : ISectionCrackedLogic
    {
        static readonly IStressLogic stressLogic = new StressLogic();
        public IForceTuple Tuple { get; set; }
        public IEnumerable<INdm> NdmCollection { get; set; }
        public Accuracy Accuracy { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public HoleSectionCrackedLogic()
        {
            if (Accuracy is null)
            {
                Accuracy = new Accuracy()
                {
                    IterationAccuracy = 0.001d,
                    MaxIterationCount = 10000
                };
            }
        }
        public bool IsSectionCracked()
        {
            TraceLogger?.AddMessage($"Calculator type: {GetType()}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"It is assumed, that cracks appearence if: cross-section has elementary parts of concrete and strain of concrete greater than limit value");
            TraceLogger?.AddMessage($"Force combination for cracking check");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(Tuple));
            var inputData = new ForceTupleInputData()
            {
                Accuracy = Accuracy,
                Tuple = Tuple,
                NdmCollection = NdmCollection
            };
            var calculator = new ForceTupleCalculator(inputData);
            if (TraceLogger is not null)
            {
                calculator.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
            }
            calculator.Run();
            var calcResult = calculator.Result as ForcesTupleResult;
            if (calcResult.IsValid == false)
            {
                TraceLogger?.AddMessage($"Result of calculation is not valid {calcResult.Description}", TraceLogStatuses.Error);
                throw new StructureHelperException(ErrorStrings.ResultIsNotValid + ": Result of Section Calculation is not valid");
            }
            var strainMatrix = calcResult.LoaderResults.ForceStrainPair.StrainMatrix;
            var isSectionCracked = stressLogic.IsSectionCracked(strainMatrix, NdmCollection);
            if (isSectionCracked == true)
            {
                TraceLogger?.AddMessage($"Cracks are appeared in cross-section for current force combination");
            }
            else
            {
                TraceLogger?.AddMessage($"Cracks are not appeared in cross-section for current force combination");
            }
            return isSectionCracked;
        }
    }
}
