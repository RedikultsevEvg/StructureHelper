using LoaderCalculator;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.SourceData;
using LoaderCalculator.Tests.Infrastructures.Logics;
using NUnit.Framework;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System.Collections.Generic;
using System.Threading;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.Models.Primitives;
using StructureHelper.Windows.ViewModels.PrimitiveProperties;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperCommon.Infrastructures.Settings;

namespace StructureHelperTests.ViewModelTests
{
    public class NdmPrimitiveTests
    {
        [Test]
        public void Rectangle_Run_ShouldPass()
        {
            //Arrange
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40);
            var primitive = new RectanglePrimitive(material);
            var primitiveBase = new RectangleViewPrimitive(primitive);
            //Act
            var vm = new PrimitivePropertiesViewModel(primitiveBase, new CrossSectionRepository());
            //Assert
            Assert.NotNull(vm);
        }

        public void Circle_Run_ShouldPass()
        {
            //Arrange
            ProgramSetting.NatSystem = NatSystems.RU;
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40);
            var primitive = new CirclePrimitive() { HeadMaterial = material};
            var primitiveBase = new CircleViewPrimitive(primitive);
            //Act
            var vm = new PrimitivePropertiesViewModel(primitiveBase, new CrossSectionRepository());
            //Assert
            Assert.NotNull(vm);
        }

        [Test]
        public void Point_Run_ShouldPass()
        {
            //Arrange
            ProgramSetting.NatSystem = NatSystems.RU;
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40);
            var primitive = new PointPrimitive(material);
            var primitiveBase = new PointViewPrimitive(primitive);
            //Act
            var vm = new PrimitivePropertiesViewModel(primitiveBase, new CrossSectionRepository());
            //Assert
            Assert.NotNull(vm);
        }

        public void Reinforcement_Run_ShouldPass()
        {
            //Arrange
            ProgramSetting.NatSystem = NatSystems.RU;
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40);
            var primitive = new RebarPrimitive() { HeadMaterial = material };
            var primitiveBase = new ReinforcementViewPrimitive(primitive);
            //Act
            var vm = new PrimitivePropertiesViewModel(primitiveBase, new CrossSectionRepository());
            //Assert
            Assert.NotNull(vm);
        }
    }
}
