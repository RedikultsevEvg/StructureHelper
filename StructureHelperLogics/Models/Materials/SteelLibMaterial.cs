using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials.Logics;

namespace StructureHelperLogics.Models.Materials
{
    public class SteelLibMaterial : ISteelLibMaterial
    {
        const MaterialTypes materialType = MaterialTypes.Steel;

        private IMaterialFactorLogic factorLogic => new MaterialFactorLogic(SafetyFactors);
        private readonly List<IMaterialLogic> materialLogics = ProgramSetting.MaterialLogics.Where(x => x.MaterialType == materialType).ToList();

        public Guid Id { get; }
        public ILibMaterialEntity MaterialEntity { get; set; }
        public IMaterialLogic MaterialLogic { get; set; }

        public List<IMaterialLogic> MaterialLogics => materialLogics;

        public List<IMaterialSafetyFactor> SafetyFactors { get; set; } = [];
        public double MaxPlasticStrainRatio { get; set; } = 3.0;
        public double UlsFactor { get; set; } = 1.025;
        public double SlsFactor { get; set; } = 1.0;
        public double ThicknessFactor { get; set; } = 1.0;
        public double WorkConditionFactor { get; set; } = 1.0;

        public SteelLibMaterial(Guid id)
        {
            Id = id;
            MaterialLogic = materialLogics.First();
        }


        public object Clone()
        {
            SteelLibMaterial newItem = new(Guid.NewGuid());
            var logic = new SteelLibMaterialUpdateStrategy();
            logic.Update(newItem, this);
            return newItem;
        }

        public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            return GetLoaderMaterial(limitState, calcTerm);
        }

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            SteelMaterialLogicOption options = new()
            {
                MaterialEntity = MaterialEntity,
                SafetyFactors = SafetyFactors,
                MaxPlasticStrainRatio = MaxPlasticStrainRatio,
                LimitState = limitState,
                CalcTerm = calcTerm,
                UlsFactor = UlsFactor,
                SlsFactor = SlsFactor,
                ThicknessFactor = ThicknessFactor,
                WorkConditionFactor = WorkConditionFactor,
            };
            MaterialLogic.Options = options;
            var material = MaterialLogic.GetLoaderMaterial();
            return material;
        }

        public (double Compressive, double Tensile) GetStrength(LimitStates limitState, CalcTerms calcTerm)
        {
            double baseStength = MaterialEntity.MainStrength;
            var factors = factorLogic.GetTotalFactor(limitState, calcTerm);
            double compressionFactor = 1d;
            double tensionFactor = 1d;
            compressionFactor *= factors.Compressive;
            tensionFactor *= factors.Tensile;
            double factor;
            if (limitState == LimitStates.ULS)
            {
                factor = UlsFactor;
            }
            else if (limitState == LimitStates.SLS)
            {
                factor = SlsFactor;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(limitState));
            }
            double strength = baseStength * ThicknessFactor / factor;
            return (strength * compressionFactor, strength * tensionFactor);
        }
    }
}
