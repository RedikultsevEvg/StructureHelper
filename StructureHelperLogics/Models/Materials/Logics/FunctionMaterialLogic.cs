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
            if (calcTerm == CalcTerms.ShortTerm)
            {
                if (limitState == LimitStates.ULS)
                {
                    functionMaterial.Function = functionMaterial.FunctionStorage.Func_ST_ULS;
                }
                else if (limitState == LimitStates.SLS)
                {
                    functionMaterial.Function = functionMaterial.FunctionStorage.Func_ST_SLS;
                }
                else if (limitState == LimitStates.Special)
                {
                    functionMaterial.Function = functionMaterial.FunctionStorage.Func_ST_Special;
                }
            }
            else if (calcTerm == CalcTerms.LongTerm)
            {
                if (limitState == LimitStates.ULS)
                {
                    functionMaterial.Function = functionMaterial.FunctionStorage.Func_LT_ULS;
                }
                else if (limitState == LimitStates.SLS)
                {
                    functionMaterial.Function = functionMaterial.FunctionStorage.Func_LT_SLS;
                }
                else if (limitState == LimitStates.Special)
                {
                    functionMaterial.Function = functionMaterial.FunctionStorage.Func_LT_Special;
                }
            }
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
