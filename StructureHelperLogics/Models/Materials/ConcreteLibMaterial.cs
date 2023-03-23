using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Materials.Libraries;
using LCM = LoaderCalculator.Data.Materials;
using LCMB = LoaderCalculator.Data.Materials.MaterialBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelperCommon.Infrastructures.Settings;
using LCMBML = LoaderCalculator.Data.Materials.MaterialBuilders.MaterialLogics;

namespace StructureHelperLogics.Models.Materials
{
    public class ConcreteLibMaterial : IConcreteLibMaterial
    {
        public ILibMaterialEntity MaterialEntity { get; set; }
        public List<IMaterialSafetyFactor> SafetyFactors { get; }
        public bool TensionForULS { get ; set; }
        public bool TensionForSLS { get; set; }

        private IMaterialOptionLogic optionLogic;
        private LCMBML.ITrueStrengthLogic strengthLogic;

        public ConcreteLibMaterial()
        {
            SafetyFactors = new List<IMaterialSafetyFactor>();
            SafetyFactors.AddRange(PartialCoefficientFactory.GetDefaultConcreteSafetyFactors(ProgramSetting.CodeType));
            optionLogic = new MaterialOptionLogic(new LCMB.ConcreteOptions());
            TensionForULS = false;
            TensionForSLS = true;
        }       

        public object Clone()
        {
            return new ConcreteLibMaterial() { MaterialEntity = MaterialEntity, TensionForULS = TensionForULS, TensionForSLS = TensionForSLS };
        }

        public LCM.IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            var materialOptions = optionLogic.SetMaterialOptions(MaterialEntity, limitState, calcTerm) as LCMB.ConcreteOptions;
            materialOptions.WorkInTension = false;
            if (limitState == LimitStates.ULS & TensionForULS == true)
            {
                materialOptions.WorkInTension = true;
            }
            if (limitState == LimitStates.SLS & TensionForSLS == true)
            {
                materialOptions.WorkInTension = true;
            }
            var strength = GetStrengthFactors(limitState, calcTerm);
            materialOptions.ExternalFactor.Compressive = strength.Compressive;
            materialOptions.ExternalFactor.Tensile = strength.Tensile;
            LCMB.IMaterialBuilder builder = new LCMB.ConcreteBuilder(materialOptions);
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
            strengthLogic = new LCMBML.TrueStrengthConcreteLogicSP63_2018(MaterialEntity.MainStrength);
            var strength = strengthLogic.GetTrueStrength();
            return (strength.Comressive, strength.Tensile);
        }
    }
}
