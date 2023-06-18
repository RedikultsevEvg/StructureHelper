using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCMB = LoaderCalculator.Data.Materials.MaterialBuilders;

namespace StructureHelperLogics.Models.Materials
{
    public class MaterialCommonOptionLogic : IMaterialOptionLogic
    {
        private ILibMaterialEntity materialEntity;
        private LimitStates limitState;
        private CalcTerms calcTerm;

        public MaterialCommonOptionLogic(ILibMaterialEntity materialEntity, LimitStates limitState, CalcTerms calcTerm)
        {
            this.materialEntity = materialEntity;
            this.limitState = limitState;
            this.calcTerm = calcTerm;
        }

        public void SetMaterialOptions(LCMB.IMaterialOptions materialOptions)
        {
            materialOptions.Strength = materialEntity.MainStrength;
            if (materialEntity.CodeType == CodeTypes.EuroCode_2_1990)
            {
                materialOptions.CodesType = LCMB.CodesType.EC2_1990;
            }
            else if (materialEntity.CodeType == CodeTypes.SP63_2018)
            {
                materialOptions.CodesType = LCMB.CodesType.SP63_2018;
            }
            else { throw new StructureHelperException($"{ErrorStrings.ObjectTypeIsUnknown} : {materialOptions.CodesType}"); }
            if (limitState == LimitStates.ULS) { materialOptions.LimitState = LCMB.LimitStates.Collapse; }
            else if (limitState == LimitStates.SLS) { materialOptions.LimitState = LCMB.LimitStates.ServiceAbility; }
            else if (limitState == LimitStates.Special) { materialOptions.LimitState = LCMB.LimitStates.Special; }
            else { throw new StructureHelperException(ErrorStrings.LimitStatesIsNotValid); }
            if (calcTerm == CalcTerms.ShortTerm) { materialOptions.IsShortTerm = true; }
            else if (calcTerm == CalcTerms.LongTerm) { materialOptions.IsShortTerm = false; }
            else { throw new StructureHelperException(ErrorStrings.LoadTermIsNotValid); }
        }
    }
}
