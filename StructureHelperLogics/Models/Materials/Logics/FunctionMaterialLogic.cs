using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;

namespace StructureHelperLogics.Models.Materials.Logics
{
    internal class FunctionMaterialLogic : IFunctionMaterialLogic
    {
        private List<double> parameters;
        private IFunctionMaterial functionMaterial;
        public IMaterial GetLoaderMaterial(IFunctionMaterial functionMaterial, LimitStates limitState, CalcTerms calcTerm, double factor = 1d)
        {
            IMaterial material = new Material();
            material.InitModulus = functionMaterial.Modulus;
            functionMaterial.Function = functionMaterial.MaterialSettings
                .Where(
                        x => x.LimitState.Equals(limitState) 
                             &&
                             x.CalcTerm.Equals(calcTerm)
                      )
                .Select(x => x.Function)
                .First();
            this.functionMaterial = functionMaterial;
            material.Diagram = GetStressByStrain;
            return material;
        }
        private double GetStressByStrain(IEnumerable<double> parameters1, double strain)
        {
            return functionMaterial.Function.GetByX(strain);
        }
    }
}
