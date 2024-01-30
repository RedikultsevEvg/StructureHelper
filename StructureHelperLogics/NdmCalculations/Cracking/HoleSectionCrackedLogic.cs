using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using StructureHelperCommon.Infrastructures.Exceptions;
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
        public HoleSectionCrackedLogic()
        {
            if (Accuracy is null)
            {
                Accuracy = new Accuracy() { IterationAccuracy = 0.001d, MaxIterationCount = 10000 };
            }
        }
        public bool IsSectionCracked()
        {
            var inputData = new ForceTupleInputData()
            {
                Accuracy = Accuracy,
                Tuple = Tuple,
                NdmCollection = NdmCollection
            };
            var calculator = new ForceTupleCalculator(inputData);
            calculator.Run();
            var calcResult = calculator.Result as ForcesTupleResult;
            if (calcResult.IsValid == false)
            {
                throw new StructureHelperException(ErrorStrings.ResultIsNotValid + ": Result of Section Calculation is not valid");
            }
            var strainMatrix = calcResult.LoaderResults.ForceStrainPair.StrainMatrix;
            return stressLogic.IsSectionCracked(strainMatrix, NdmCollection);
        }
    }
}
