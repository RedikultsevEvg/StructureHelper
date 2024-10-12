using LoaderCalculator.Data.Materials;
using NUnit.Framework;
using System.Linq;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using StructureHelperCommon.Infrastructures.Enums;
using LoaderCalculator.Logics.Geometry;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelper.Models.Materials;
using Moq;

namespace StructureHelperTests.UnitTests.Ndms.Triangulations
{
    public class RectangleTriangulationTest
    {
        private Mock<IHeadMaterial> materialMock;
        private ITriangulatePrimitiveLogic triangulateLogic;

        [SetUp]
        public void Setup()
        {
            materialMock = new Mock<IHeadMaterial>();
        }
        //Участок по центру
        [TestCase(0d, 0d, 1.0d, 1.0d, 0d, 0.02d, 1, 50 * 50, -0.49d, -0.49d)]
        //Участок со смещением от центра
        [TestCase(2d, 2d, 1.0d, 1.0d, 0d, 0.02d, 1, 50 * 50, 1.51d, 1.51d)]
        [TestCase(2d, 1d, 1.0d, 1.0d, 0d, 0.02d, 1, 50 * 50, 1.51d, 0.51d)]
        //Участок с поворотом на 1 радиан
        [TestCase(0d, 0d, 1.0d, 1.0d, 1d, 0.02d, 1, 50 * 50, 0.14757265268048089d, -0.67706891243125777d)]
        //Участок со смещением и поворотом на 1 радиан
        [TestCase(2d, 2d, 1.0d, 1.0d, 1d, 0.02d, 1, 50 * 50, -0.45476470519903262d, 2.0864776689208147d)]
        public void Run_ShouldPass (double centerX, double centerY, double width, double height, double angle, double ndmMaxSize, int ndmMinDivision, int expectedCount, double expectedFirstCenterX, double expectedFirstCenterY)
        {
            //Arrange
            materialMock
                .Setup(x => x.GetLoaderMaterial(It.IsAny<LimitStates>(), It.IsAny<CalcTerms>()))
                .Returns(new Material());

            IPoint2D center = new Point2D { X = centerX, Y = centerY };
            IRectangleShape rectangle = new RectangleShape { Width = width, Height = height, Angle = angle };
            var options = new RectangleTriangulationLogicOptions(center, rectangle, ndmMaxSize, ndmMinDivision)
            {
                triangulationOptions = new TriangulationOptions() { LimiteState = LimitStates.ULS, CalcTerm = CalcTerms.ShortTerm },
                HeadMaterial = materialMock.Object
            };
            var logic = new RectangleTriangulationLogic(options);
            //Act
            var result = logic.GetNdmCollection();
            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedCount, result.Count());
            var firstNdm = result.First();
            Assert.AreEqual(expectedFirstCenterX, firstNdm.CenterX);
            Assert.AreEqual(expectedFirstCenterY, firstNdm.CenterY);
        }
        [TestCase(0d, 0d, 1.0d, 1.0d, true, 1d, 3020418448.8512335d, 3020418448.8512335d)]
        [TestCase(0d, 0d, 1.0d, 1.0d, false, 0.94d, 3013186843.9241991d, 3004124682.3615651d)]
        [TestCase(0.1d, 0.2d, 1.0d, 1.0d, true, 1d, 3020418448.8512335d, 3020418448.8512335d)]
        [TestCase(0.1d, 0.2d, 1.0d, 1.0d, false, 0.94d, 2920637108.8157182d, 2980987248.5842943d)]
        public void Run_Shouldpass_RectOpeningRect(double centerX, double centerY, double width, double height, bool triangOpening, double expectedArea, double expectedMomX, double expectedMomY)
        {
            //Arrange
            ProgramSetting.NatSystem = NatSystems.RU;
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40);
            var mainBlock = new RectanglePrimitive()
            {
                Width = width,
                Height = height,
            };
            mainBlock.NdmElement.HeadMaterial = material;
            mainBlock.Center.X = centerX;
            mainBlock.Center.Y = centerY;
            mainBlock.VisualProperty.ZIndex = 0;
            var opening = new RectanglePrimitive()
            {
                Width = 0.3d,
                Height = 0.2d
            };
            opening.DivisionSize.ClearUnderlying = true;
            opening.NdmElement.HeadMaterial = material;
            opening.NdmElement.Triangulate = triangOpening;
            opening.VisualProperty.ZIndex = 1;
            var primitives = new List<INdmPrimitive>() { mainBlock, opening };
            //Act
            triangulateLogic = new TriangulatePrimitiveLogic()
            {
                Primitives = primitives,
                LimitState = LimitStates.ULS,
                CalcTerm = CalcTerms.ShortTerm
            };
            var ndms = triangulateLogic.GetNdms();
            //Assert
            var area = ndms.Sum(x => x.Area);
            var moments = GeometryOperations.GetReducedMomentsOfInertia(ndms);
            Assert.AreEqual(expectedArea, area, 0.001d);
            Assert.AreEqual(expectedMomX, moments.EIx, 0.001d);
            Assert.AreEqual(expectedMomY, moments.EIy, 1d);
        }
        [TestCase(0d, 0d, 1.0d, 1.0d, true, 1d, 3020017308.3574591d, 3020017308.3574591d)]
        [TestCase(0d, 0d, 1.0d, 1.0d, false, 0.92839999999991407d, 3005633713.5049105d, 3005633713.5049105d)]
        [TestCase(0.1d, 0.2d, 1.0d, 1.0d, true, 1d, 3018642205.1351919d, 3019673532.5518913d)]
        [TestCase(0.1d, 0.2d, 1.0d, 1.0d, false, 0.92839999999991407d, 2893811090.8611836d, 2977678057.8439965d)]
        public void Run_Shouldpass_RectOpeningCircle(double centerX, double centerY, double width, double height, bool triangOpening, double expectedArea, double expectedMomX, double expectedMomY)
        {
            //Arrange
            ProgramSetting.NatSystem = NatSystems.RU;
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40);
            var mainBlock = new RectanglePrimitive()
            {
                Width = width,
                Height = height
            };
            mainBlock.NdmElement.HeadMaterial = material;
            mainBlock.Center.X = centerX;
            mainBlock.Center.Y = centerY;
            mainBlock.VisualProperty.ZIndex = 0;
            var opening = new EllipsePrimitive()
            {
                Width = 0.3d
            };
            opening.DivisionSize.ClearUnderlying = true;
            opening.NdmElement.HeadMaterial = material;
            opening.NdmElement.Triangulate = triangOpening;
            opening.VisualProperty.ZIndex = 1;
            var primitives = new List<INdmPrimitive>()
            {
                mainBlock,
                opening
            };
            //Act
            triangulateLogic = new TriangulatePrimitiveLogic()
            {
                Primitives = primitives,
                LimitState = LimitStates.ULS,
                CalcTerm = CalcTerms.ShortTerm
            };
            var ndms = triangulateLogic.GetNdms();
            //Assert
            var area = ndms.Sum(x => x.Area);
            var moments = GeometryOperations.GetReducedMomentsOfInertia(ndms);
            Assert.AreEqual(expectedArea, area, 0.001d);
            Assert.AreEqual(expectedMomX, moments.EIx, 0.001d);
            Assert.AreEqual(expectedMomY, moments.EIy, 1d);
        }
        [TestCase(0d, 0d, 1.0d, true, 0.78079430967489682d, 1777730450.3666615d, 1776732530.5957441d)]
        [TestCase(0d, 0d, 1.0d, false, 0.72079430967490343d, 1770498845.4396176d, 1760438764.1059904d)]
        [TestCase(0.1d, 0.2d, 1.0d, true, 0.78386909642355929d, 1772956158.4523892d, 1777255335.2898147d)]
        [TestCase(0.1d, 0.2d, 1.0d, false, 0.72386909642356589d, 1670848320.3763092d, 1737334754.3254938d)]
        public void Run_Shouldpass_CircleOpeningRect(double centerX, double centerY, double diameter, bool triangOpening, double expectedArea, double expectedMomX, double expectedMomY)
        {
            //Arrange
            ProgramSetting.NatSystem = NatSystems.RU;
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40);
            var mainBlock = new EllipsePrimitive() { Width = diameter};
            mainBlock.NdmElement.HeadMaterial = material;
            mainBlock.Center.X = centerX;
            mainBlock.Center.Y = centerY;
            mainBlock.VisualProperty.ZIndex = 0;
            var opening = new RectanglePrimitive()
            {
                Width = 0.3d,
                Height = 0.2d
            };
            opening.DivisionSize.ClearUnderlying = true;
            opening.NdmElement.HeadMaterial = material;
            opening.NdmElement.Triangulate = triangOpening;
            opening.VisualProperty.ZIndex = 1;
            var primitives = new List<INdmPrimitive>() { mainBlock, opening };
            //Act
            triangulateLogic = new TriangulatePrimitiveLogic()
            {
                Primitives = primitives,
                LimitState = LimitStates.ULS,
                CalcTerm = CalcTerms.ShortTerm
            };
            var ndms = triangulateLogic.GetNdms();
            //Assert
            var area = ndms.Sum(x => x.Area);
            var moments = GeometryOperations.GetReducedMomentsOfInertia(ndms);
            Assert.AreEqual(expectedArea, area, 0.001d);
            Assert.AreEqual(expectedMomX, moments.EIx, 0.001d);
            Assert.AreEqual(expectedMomY, moments.EIy, 1d);
        }
    }
}
