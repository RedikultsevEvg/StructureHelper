using LoaderCalculator;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.SourceData;
using LoaderCalculator.Tests.Infrastructures.Logics;
using NUnit.Framework;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System.Collections.Generic;
using System.Threading;
using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.NdmPrimitives;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperTests.FunctionalTests.Ndms.RCSections
{
    //В тесте создается 1 прямоугольник размером 400х600мм (0.4х0.6м).
    //Материал прямоугольника - бетон, пока задается жестко в модели прямоугольника, надо сделать чтобы редактировалось через материалы
    //Также создается 4 точки, соответствующие арматуре, материал - арматура, также пока жестко задается в свойствах точки
    //Входными параметрами являтся 3 параметра усилий - Mx, My, Nz
    //Выходными параметрами являются 3 параметра перемещений - Kx, Ky, EpsilonZ
    //
    public class RCSectionFromNdmPrimitiveTest
    {
        //Theoretical limit momemt Mx = 43kN*m
        [TestCase(0.000113, 0.000494, 10e3, 0d, 0d, 0.00084665917358052976d, 0.0d, 0.00020754144937701132d)]
        [TestCase(0.000113, 0.000494, 40e3, 0d, 0d, 0.0033939850380287412d, 0d, 0.00082989880025069202d)]
        [TestCase(0.000113, 0.000494, 42e3, 0d, 0d, 0.0056613831873867241d, 0d, 0.0014291081844183839d)]
        //Theoretical limit momemt Mx = -187kN*m
        [TestCase(0.000113, 0.000494, -50e3, 0d, 0d, -0.0011229555729294297d, 0d, 0.00021353225742956321d)]
        [TestCase(0.000113, 0.000494, -180e3, 0d, 0d, -0.0098365950945499738d, 0d, 0.0022035516889170013d)]
        [TestCase(0.000113, 0.000494, -183e3, 0d, 0d, -0.021718635290382458d, 0d, 0.0053526701372818789d)]
        public void Run_ShouldPass(double topArea, double bottomArea, double mx, double my, double nz, double expectedKx, double expectedKy, double expectedEpsilonZ)
        {
            //Arrange
            double width = 0.4;
            double height = 0.6;
            //Коллекция для хранения элементарных участков
            var ndmCollection = new List<INdm>();
            //Настройки триангуляции, пока опции могут быть только такие
            ITriangulationOptions options = new TriangulationOptions { LimiteState = StructureHelperLogics.Infrastructures.CommonEnums.LimitStates.Collapse, CalcTerm = StructureHelperLogics.Infrastructures.CommonEnums.CalcTerms.ShortTerm };
            var ndmPrimitives = new List<INdmPrimitive>();
            //Добавляем прямоугольник бетонного сечения
            var concreteRectangle = new RectanglePrimitive(new Center { X = 0, Y = 0 }, new Rectangle { Width = width, Height = height, Angle = 0 });
            ndmPrimitives.Add(concreteRectangle.GetNdmPrimitive());
            //Добавляем 4 точки для арматуры
            // 0.05 - величина защитного слоя (расстояние от грани прямоугольника до центра арматуры
            //С площадью нижней арматуры
            var leftBottomReinforcementPoint = new PointPrimitive(new Center { X = -width / 2 + 0.05d, Y = -height / 2 + 0.05 }, new Point { Area = bottomArea });
            ndmPrimitives.Add(leftBottomReinforcementPoint.GetNdmPrimitive());
            var rightBottomReinforcementPoint = new PointPrimitive(new Center { X = width / 2 - 0.05d, Y = -height / 2 + 0.05 }, new Point { Area = bottomArea });
            ndmPrimitives.Add(rightBottomReinforcementPoint.GetNdmPrimitive());
            //С площадью верхней арматуры
            var leftTopReinforcementPoint = new PointPrimitive(new Center { X = -width / 2 + 0.05d, Y = height / 2 - 0.05 }, new Point { Area = topArea });
            ndmPrimitives.Add(leftTopReinforcementPoint.GetNdmPrimitive());
            var rightTopReinforcementPoint = new PointPrimitive(new Center { X = width / 2 - 0.05d, Y = height / 2 - 0.05 }, new Point { Area = topArea });
            ndmPrimitives.Add(rightTopReinforcementPoint.GetNdmPrimitive());
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
            //Act
            calculator.Run(loaderData, new CancellationToken());
            var results = calculator.Result;
            //Assert
            Assert.NotNull(results);
            //Основной результат расчета - матрица (матрица-столбец) деформаций, состоящая из трех членов
            var strainMatrix = results.StrainMatrix;
            Assert.NotNull(strainMatrix);
            Assert.AreEqual(expectedKx, strainMatrix.Kx, ExpectedProcessor.GetAccuracyForExpectedValue(expectedKx));
            Assert.AreEqual(expectedKy, strainMatrix.Ky, ExpectedProcessor.GetAccuracyForExpectedValue(expectedKy));
            Assert.AreEqual(expectedEpsilonZ, strainMatrix.EpsZ, ExpectedProcessor.GetAccuracyForExpectedValue(expectedEpsilonZ));
        }
    }
}
