using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System.Diagnostics.Eventing.Reader;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <inheritdoc/>
    internal class IsSectionCrackedByForceLogic : IIsSectionCrackedByForceLogic
    {
        static readonly IStressLogic stressLogic = new StressLogic();
        /// <inheritdoc/>
        public IForceTuple Tuple { get; set; }
        public IEnumerable<INdm> CheckedNdmCollection { get; set; }
        public IEnumerable<INdm> SectionNdmCollection { get; set; }
        public Accuracy Accuracy { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public IsSectionCrackedByForceLogic()
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
                ForceTuple = Tuple,
                NdmCollection = SectionNdmCollection
            };
            var calculator = new ForceTupleCalculator() { InputData = inputData };
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
            IEnumerable<INdm> checkedNdmCollection;
            var isSectionCracked = stressLogic.IsSectionCracked(strainMatrix, CheckedNdmCollection);
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
