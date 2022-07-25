using Autofac;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LoaderCalculator;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.SourceData;
using StructureHelper;
using StructureHelper.Services;
using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.NdmPrimitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Infrastructures.CommonEnums;

namespace StructureHelperLogics.Services
{
    public class CalculationService
    {
        public IStrainMatrix GetPrimitiveStrainMatrix(double topArea, double bottomArea, RectanglePrimitive concreteRectangle, double mx, double my, double nz)
        {
            var ndmPrimitives = new List<INdmPrimitive>();
            //Добавляем прямоугольник бетонного сечения

            ndmPrimitives.Add(concreteRectangle.GetNdmPrimitive());
            
            using (var scope = App.Container.BeginLifetimeScope())
            {
                var primitiveService = scope.Resolve<PrimitiveService>();
                //Добавляем точки внутри прямоугольника
                ndmPrimitives.AddRange(primitiveService.GetInnerPoints(concreteRectangle).Select(x=>x.GetNdmPrimitive()));
            }

            //Коллекция для хранения элементарных участков
            var ndmCollection = new List<INdm>();
            //Настройки триангуляции, пока опции могут быть только такие
            ITriangulationOptions options = new TriangulationOptions
            {
                LimiteState = LimitStates.Collapse,
                CalcTerm = CalcTerms.ShortTerm
            };

            //Формируем коллекцию элементарных участков для расчета в библитеке (т.е. выполняем триангуляцию)
            ndmCollection.AddRange(Triangulation.GetNdms(ndmPrimitives, options));

            var calculator = new Calculator();
            var calculationData = new LoaderOptions
            {
                Preconditions = new Preconditions
                {
                    ConditionRate = 0.01,
                    MaxIterationCount = 100,
                    StartForceMatrix = new ForceMatrix
                    {
                        Mx = mx,
                        My = my,
                        Nz = nz
                    }
                },
                NdmCollection = ndmCollection
            };
            calculator.Run(calculationData, new CancellationToken());
            var results = calculator.Result;
            return results.StrainMatrix;
        }
    }
}
