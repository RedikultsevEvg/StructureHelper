using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperTests.UnitTests.MaterialTests
{
    public class MaterialStrengthTest
    {
        [TestCase(HeadmaterialType.Reinforcement400, CodeTypes.SP63_2018, LimitStates.ULS, CalcTerms.ShortTerm, 347826086.95652175d, 347826086.95652175d)]
        [TestCase(HeadmaterialType.Reinforcement400, CodeTypes.SP63_2018, LimitStates.SLS, CalcTerms.ShortTerm, 400000000d, 400000000d)]
        [TestCase(HeadmaterialType.Reinforecement500, CodeTypes.SP63_2018, LimitStates.ULS, CalcTerms.ShortTerm, 400000000.0d, 434782608.69565225d)]
        [TestCase(HeadmaterialType.Reinforecement500, CodeTypes.SP63_2018, LimitStates.ULS, CalcTerms.LongTerm, 434782608.69565225d, 434782608.69565225d)]
        [TestCase(HeadmaterialType.Reinforecement500, CodeTypes.SP63_2018, LimitStates.SLS, CalcTerms.ShortTerm, 5e8d, 5e8d)]
        [TestCase(HeadmaterialType.Reinforecement500, CodeTypes.SP63_2018, LimitStates.SLS, CalcTerms.ShortTerm, 5e8d, 5e8d)]
        [TestCase(HeadmaterialType.Concrete40, CodeTypes.SP63_2018, LimitStates.ULS, CalcTerms.ShortTerm, 22461538.46153846d, 1395297.0017909051d)]
        [TestCase(HeadmaterialType.Concrete40, CodeTypes.SP63_2018, LimitStates.ULS, CalcTerms.LongTerm, 20215384.615384616d, 1255767.3016118146d)]
        [TestCase(HeadmaterialType.Concrete40, CodeTypes.SP63_2018, LimitStates.SLS, CalcTerms.ShortTerm, 29200000.0d, 2092945.5026863578d)]
        [TestCase(HeadmaterialType.Concrete40, CodeTypes.SP63_2018, LimitStates.SLS, CalcTerms.LongTerm, 29200000.0d, 2092945.5026863578d)]
        public void Run_ShouldPass(HeadmaterialType headmaterialType, CodeTypes code, LimitStates limitState, CalcTerms calcTerm, double expectedCompressive, double expectedTensile)
        {
            //Arrange
            ProgramSetting.NatSystem = NatSystems.RU;
            var material = HeadMaterialFactory.GetHeadMaterial(headmaterialType);
            var libMaterial = material.HelperMaterial as ILibMaterial;
            //Act
            var compressive = libMaterial.GetStrength(limitState, calcTerm).Compressive;
            var tensile = libMaterial.GetStrength(limitState, calcTerm).Tensile;
            //Assert
            Assert.AreEqual(expectedCompressive, compressive, 1d);
            Assert.AreEqual(expectedTensile, tensile, 1d);
        }
    }
}
