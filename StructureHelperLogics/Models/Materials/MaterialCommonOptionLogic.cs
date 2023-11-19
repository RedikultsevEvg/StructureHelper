using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using LCMB = LoaderCalculator.Data.Materials.MaterialBuilders;

namespace StructureHelperLogics.Models.Materials
{
    public class MaterialCommonOptionLogic : IMaterialOptionLogic
    {
        private IMaterialLogicOptions options;

        public MaterialCommonOptionLogic(IMaterialLogicOptions options)
        {
            this.options = options;
        }

        public void SetMaterialOptions(LCMB.IMaterialOptions materialOptions)
        {
            materialOptions.Strength = options.MaterialEntity.MainStrength;
            if (options.MaterialEntity.CodeType == CodeTypes.EuroCode_2_1990)
            {
                materialOptions.CodesType = LCMB.CodesType.EC2_1990;
            }
            else if (options.MaterialEntity.CodeType == CodeTypes.SP63_2018)
            {
                materialOptions.CodesType = LCMB.CodesType.SP63_2018;
            }
            else { throw new StructureHelperException($"{ErrorStrings.ObjectTypeIsUnknown} : {materialOptions.CodesType}"); }
            if (options.LimitState == LimitStates.ULS)
            {
                materialOptions.LimitState = LCMB.LimitStates.Collapse;
            }
            else if (options.LimitState == LimitStates.SLS)
            {
                materialOptions.LimitState = LCMB.LimitStates.ServiceAbility;
            }
            else if (options.LimitState == LimitStates.Special)
            {
                materialOptions.LimitState = LCMB.LimitStates.Special;
            }
            else { throw new StructureHelperException(ErrorStrings.LimitStatesIsNotValid); }
            if (options.CalcTerm == CalcTerms.ShortTerm) { materialOptions.IsShortTerm = true; }
            else if (options.CalcTerm == CalcTerms.LongTerm) { materialOptions.IsShortTerm = false; }
            else { throw new StructureHelperException(ErrorStrings.LoadTermIsNotValid); }
        }
    }
}
