using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Logics;
using StructureHelperCommon.Services;


namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class HeadMaterialsCheckLogic : CheckEntityLogic<IEnumerable<IHeadMaterial>>
    {
        private ICheckEntityLogic<IUserMaterial> userMaterialCheckEntityLogic;


        private ICheckEntityLogic<IUserMaterial> UserMaterialCheckEntityLogic => userMaterialCheckEntityLogic ??= new UserMaterialCheckLogic() { TraceLogger = TraceLogger};
        public IEnumerable<LimitStates> LimitStates { get; }
        public IEnumerable<CalcTerms> CalcTerms { get; }

        public HeadMaterialsCheckLogic(IEnumerable<LimitStates> limitStates, IEnumerable<CalcTerms> calcTerms)
        {
            LimitStates = limitStates;
            CalcTerms = calcTerms;
        }

        public override bool Check()
        {
            bool result = true;
            CheckObject.ThrowIfNull(Entity);
            foreach (var headMaterial in Entity)
            {
                if (headMaterial.HelperMaterial is IUserMaterial userMaterial)
                {
                    UserMaterialCheckEntityLogic.Entity = userMaterial;
                    if (UserMaterialCheckEntityLogic.Check() == false)
                    {
                        result = false;
                        CheckResult += UserMaterialCheckEntityLogic.CheckResult;
                    }
                }
                foreach (var limitState in LimitStates)
                {
                    foreach (var calcTerm in CalcTerms)
                    {
                        try
                        {
                            var material = headMaterial.GetLoaderMaterial(limitState, calcTerm);
                            material = headMaterial.GetCrackedLoaderMaterial(limitState, calcTerm);
                        }
                        catch (Exception ex)
                        {
                            TraceMessage($"Unknown error of creating of material: {ex.Message}, Limit state: {limitState}, Calc term: {calcTerm}");
                            result = false;
                        }
                    }
                }
            }
            return result;
        }
    }
}
