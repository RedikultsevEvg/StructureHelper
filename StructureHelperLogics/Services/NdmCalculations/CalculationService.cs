using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.SourceData;
using LoaderCalculator;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using StructureHelperLogics.Models.Calculations.CalculationsResults;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.Services.NdmCalculations
{
    public class CalculationService
    {
        private ICalculationProperty calculationProperty;

        public IStrainMatrix GetPrimitiveStrainMatrix(INdmPrimitive[] ndmPrimitives, double mx, double my, double nz)
        {
            var ndmCollection = new List<INdm>();
            ndmCollection.AddRange(NdmPrimitivesService.GetNdms(ndmPrimitives, calculationProperty.LimitState, calculationProperty.CalcTerm));
            var loaderData = new LoaderOptions
            {
                Preconditions = new Preconditions
                {
                    ConditionRate = calculationProperty.Accuracy.IterationAccuracy,
                    MaxIterationCount = calculationProperty.Accuracy.MaxIterationCount,
                    StartForceMatrix = new ForceMatrix { Mx = mx, My = my, Nz = nz }
                },
                NdmCollection = ndmCollection
            };
            var calculator = new Calculator();
            calculator.Run(loaderData, new CancellationToken());
            return calculator.Result.StrainMatrix;
        }

        public List<ICalculationResult> GetCalculationResults(IEnumerable<INdm> ndms)
        {
            List<ICalculationResult> results = new List<ICalculationResult>();
            foreach (var forceCombinations in calculationProperty.ForceCombinations)
            {
                var forceMatrix = forceCombinations.ForceMatrix;
                results.Add(GetCalculationResult(forceMatrix, ndms, calculationProperty.Accuracy.IterationAccuracy, calculationProperty.Accuracy.MaxIterationCount));
            }
            return results;
        }

        public ICalculationResult GetCalculationResult(IForceMatrix forceMatrix, IEnumerable<INdm> ndmCollection, double accuracyRate, int maxIterationCount)
        {
            try
            {
                var loaderData = new LoaderOptions
                {
                    Preconditions = new Preconditions
                    {
                        ConditionRate = accuracyRate,
                        MaxIterationCount = maxIterationCount,
                        StartForceMatrix = forceMatrix
                    },
                    NdmCollection = ndmCollection
                };
                var calculator = new Calculator();
                calculator.Run(loaderData, new CancellationToken());
                var result = calculator.Result;
                if (result.AccuracyRate <= accuracyRate) { return new CalculationResult() { IsValid = true, Desctription = "Analisys is done succsefully", LoaderResults = result }; }
                else { return new CalculationResult() { IsValid = false, Desctription = "Required accuracy rate has not achived", LoaderResults = result }; }
            }
            catch (Exception ex)
            {
                return new CalculationResult() { IsValid = false, Desctription = $"Error is appeared due to analysis. Error: {ex}" };
            }
        }

        public CalculationService(ICalculationProperty property)
        {
            calculationProperty = property;
        }
        public CalculationService()
        {

        }
    }
}
