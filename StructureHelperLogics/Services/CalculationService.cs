using System.Collections.Generic;
using System.Threading;
using LoaderCalculator;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.SourceData;
using StructureHelperCommon.Models.Entities;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Infrastructures.CommonEnums;

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
    }
}
