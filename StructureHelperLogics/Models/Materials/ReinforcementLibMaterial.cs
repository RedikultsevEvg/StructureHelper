using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCM = LoaderCalculator.Data.Materials;
using LCMB = LoaderCalculator.Data.Materials.MaterialBuilders;
using LCMBML = LoaderCalculator.Data.Materials.MaterialBuilders.MaterialLogics;

namespace StructureHelperLogics.Models.Materials
{
    public class ReinforcementLibMaterial : IReinforcementLibMaterial
    {
        public ILibMaterialEntity MaterialEntity { get; set; }
        public List<IMaterialSafetyFactor> SafetyFactors { get; }

        private IMaterialOptionLogic optionLogic;
        private LCMBML.ITrueStrengthLogic strengthLogic;

        public ReinforcementLibMaterial()
        {
            SafetyFactors = new List<IMaterialSafetyFactor>();
            optionLogic = new MaterialOptionLogic(new LCMB.ReinforcementOptions());
        }

        public object Clone()
        {
            return new ReinforcementLibMaterial() { MaterialEntity = MaterialEntity};
        }

        public LCM.IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            var materialOptions = optionLogic.SetMaterialOptions(MaterialEntity, limitState, calcTerm);
            var strength = GetStrengthFactors(limitState, calcTerm);
            materialOptions.ExternalFactor.Compressive = strength.Compressive;
            materialOptions.ExternalFactor.Tensile = strength.Tensile;
            LCMB.IMaterialBuilder builder = new LCMB.ReinforcementBuilder(materialOptions);
            LCMB.IBuilderDirector director = new LCMB.BuilderDirector(builder);
            return director.BuildMaterial();
        }

        public (double Compressive, double Tensile) GetStrengthFactors(LimitStates limitState, CalcTerms calcTerm)
        {
            double compressionVal = 1d;
            double tensionVal = 1d;
            foreach (var item in SafetyFactors.Where(x => x.Take == true))
            {
                compressionVal *= item.GetFactor(StressStates.Compression, calcTerm, limitState);
                tensionVal *= item.GetFactor(StressStates.Tension, calcTerm, limitState);
            }
            return (compressionVal, tensionVal);
        }

        public (double Compressive, double Tensile) GetStrength(LimitStates limitState, CalcTerms calcTerm)
        {
            strengthLogic = new LCMBML.TrueStrengthReinforcementLogic(MaterialEntity.MainStrength);
            var strength = strengthLogic.GetTrueStrength();
            return (strength.Comressive, strength.Tensile);
        }
    }
}
