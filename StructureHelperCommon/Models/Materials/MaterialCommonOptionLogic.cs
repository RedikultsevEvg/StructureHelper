using LoaderCalculator.Data.Materials.MaterialBuilders;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials.Libraries;
using LCMB = LoaderCalculator.Data.Materials.MaterialBuilders;
using SHEnums = StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.Materials
{
    public class MaterialCommonOptionLogic : IMaterialOptionLogic
    {
        private IMaterialLogicOptions options;
        private FactorLogic factorLogic;

        public MaterialCommonOptionLogic(IMaterialLogicOptions options)
        {
            this.options = options;
        }

        public void SetMaterialOptions(LCMB.IMaterialOptions materialOptions)
        {
            materialOptions.InitModulus = options.MaterialEntity.InitModulus;
            materialOptions.Strength = options.MaterialEntity.MainStrength;
            ProcessCodeType(materialOptions);
            ProcessLimitState(materialOptions);
            ProcessCalcTerm(materialOptions);
            ProcessExternalFactors(materialOptions);
        }

        private void ProcessExternalFactors(IMaterialOptions materialOptions)
        {
            factorLogic = new FactorLogic(options.SafetyFactors);
            var strength = factorLogic.GetTotalFactor(options.LimitState, options.CalcTerm);
            materialOptions.ExternalFactor.Compressive = strength.Compressive;
            materialOptions.ExternalFactor.Tensile = strength.Tensile;
        }

        private void ProcessCalcTerm(IMaterialOptions materialOptions)
        {
            if (options.CalcTerm == CalcTerms.ShortTerm)
            {
                materialOptions.IsShortTerm = true;
            }
            else if (options.CalcTerm == CalcTerms.LongTerm)
            {
                materialOptions.IsShortTerm = false;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.LoadTermIsNotValid);
            }
        }

        private void ProcessLimitState(IMaterialOptions materialOptions)
        {
            if (options.LimitState == SHEnums.LimitStates.ULS)
            {
                materialOptions.LimitState = LCMB.LimitStates.Collapse;
            }
            else if (options.LimitState == SHEnums.LimitStates.SLS)
            {
                materialOptions.LimitState = LCMB.LimitStates.ServiceAbility;
            }
            else if (options.LimitState == SHEnums.LimitStates.Special)
            {
                materialOptions.LimitState = LCMB.LimitStates.Special;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.LimitStatesIsNotValid);
            }
        }

        private void ProcessCodeType(IMaterialOptions materialOptions)
        {
            if (options.MaterialEntity.CodeType == CodeTypes.EuroCode_2_1990)
            {
                materialOptions.CodesType = LCMB.CodesType.EC2_1990;
            }
            else if (options.MaterialEntity.CodeType == CodeTypes.SP63_2018)
            {
                materialOptions.CodesType = LCMB.CodesType.SP63_2018;
            }
            else
            {
                throw new StructureHelperException($"{ErrorStrings.ObjectTypeIsUnknown} : {materialOptions.CodesType}");
            }
        }
    }
}
