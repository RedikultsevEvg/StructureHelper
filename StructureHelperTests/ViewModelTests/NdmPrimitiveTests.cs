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

namespace StructureHelperTests.ViewModelTests
{
    public class NdmPrimitiveTests
    {
        [TestCase]
        public void RectanglePrimitiveTest()
        {
            //Arrange
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40, CodeTypes.SP63_13330_2018);
            var primitive = new RectanglePrimitive(material);
            var primitiveBase = new RectangleViewPrimitive(primitive);
            //Act
            var vm = new PrimitivePropertiesViewModel(primitiveBase, new CrossSectionRepository());
            //Assert
            Assert.NotNull(vm);
        }
    }
}
