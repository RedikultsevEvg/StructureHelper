using System.Collections.Generic;
using System.Threading;
using LoaderCalculator;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.SourceData;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Infrastructures.CommonEnums;
using StructureHelperLogics.Models.Calculations.CalculationsResults;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using System;
using StructureHelperLogics.Models.Primitives;

namespace StructureHelperLogics.Services
{
    public class CalculationService
    {
        public IStrainMatrix GetPrimitiveStrainMatrix(INdmPrimitive[] ndmPrimitives, double mx, double my, double nz)
        {
            //Коллекция для хранения элементарных участков
            var ndmCollection = new List<INdm>();
            //Настройки триангуляции, пока опции могут быть только такие
            ITriangulationOptions options = new TriangulationOptions { LimiteState = LimitStates.Collapse, CalcTerm = CalcTerms.ShortTerm };

            //Формируем коллекцию элементарных участков для расчета в библитеке (т.е. выполняем триангуляцию)
            ndmCollection.AddRange(Triangulation.GetNdms(ndmPrimitives, options));
            var loaderData = new LoaderOptions
            {
                Preconditions = new Preconditions
                {
                    ConditionRate = 0.01,
                    MaxIterationCount = 100,
                    StartForceMatrix = new ForceMatrix { Mx = mx, My = my, Nz = nz }
                },
                NdmCollection = ndmCollection
            };
            var calculator = new Calculator();
            calculator.Run(loaderData, new CancellationToken());
            return calculator.Result.StrainMatrix;
        }

        public List<ICalculationResult> GetCalculationResults(ICalculationProperty calculationProperty, IEnumerable<INdm> ndms)
        {
            List<ICalculationResult> results = new List<ICalculationResult>();
            foreach (var forceCombinations in calculationProperty.ForceCombinations)
            {
                var forceMatrix = forceCombinations.ForceMatrix;
                results.Add(GetCalculationResult(forceMatrix, ndms, calculationProperty.IterationProperty.Accuracy, calculationProperty.IterationProperty.MaxIterationCount));
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
                if (result.AccuracyRate <= accuracyRate) { return new CalculationResult() { IsValid = true, Desctription = "Analisys is done succsefully", LoaderResults=result };}
                else { return new CalculationResult() { IsValid = false, Desctription = "Required accuracy rate has not achived", LoaderResults = result }; }
            }
            catch (Exception ex)
            {
                return new CalculationResult() { IsValid = false, Desctription = $"Error is appeared due to analysis. Error: {ex}" };
            }
        }
    }
}
