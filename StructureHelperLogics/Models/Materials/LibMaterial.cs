using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;
using System.Collections.Generic;
using LCM = LoaderCalculator.Data.Materials;
using LCMB = LoaderCalculator.Data.Materials.MaterialBuilders;

namespace StructureHelperLogics.Models.Materials
{
    public class LibMaterial : ILibMaterial
    {
        private LCMB.IMaterialOptions materialOptions;

        public MaterialTypes MaterialType { get; set; }
        private CodeTypes codeType;

        private LimitStates limitState;
        private CalcTerms calcTerm;
        public string Name { get; set; }
        public double MainStrength { get; set; }

        public ILibMaterialEntity MaterialEntity { get; set; }

        public List<IMaterialSafetyFactor> SafetyFactors { get; set; }
        public IMaterialLogic MaterialLogic { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<IMaterialLogic> MaterialLogics => throw new NotImplementedException();

        public Guid Id { get; }

        public LibMaterial(Guid id)
        {
            Id = id;
            MaterialType = MaterialTypes.Concrete;
            codeType = CodeTypes.SP63_2018;
            Name = "New material";
        }

        public LibMaterial(MaterialTypes materialType, CodeTypes codeType, string name, double mainStrength)
            : this(Guid.NewGuid(), materialType, codeType, name, mainStrength)
        {
        }

        public LibMaterial(Guid id, MaterialTypes materialType, CodeTypes codeType, string name, double mainStrength)
        {
            Id = id;
            this.MaterialType = materialType;
            this.codeType = codeType;
            Name = name;
            MainStrength = mainStrength;
        }

        public LCM.IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            this.limitState = limitState;
            this.calcTerm = calcTerm;
            if (MaterialType == MaterialTypes.Concrete)
            {   return GetConcrete();}
            else if (MaterialType == MaterialTypes.Reinforcement)
            {   return GetReinfrocement();}
            else throw new StructureHelperException($"{ErrorStrings.ObjectTypeIsUnknown}: material type = {MaterialType}, code type = {codeType}");
        }


        private LCM.IMaterial GetReinfrocement()
        {
            materialOptions = new LCMB.ReinforcementOptions();
            SetMaterialOptions();
            LCMB.IMaterialBuilder builder = new LCMB.ReinforcementBuilder(materialOptions);
            LCMB.IBuilderDirector director = new LCMB.BuilderDirector(builder);
            return director.BuildMaterial();
        }

        private LCM.IMaterial GetConcrete()
        {
            materialOptions = new LCMB.ConcreteOptions();
            SetMaterialOptions();
            LCMB.IMaterialBuilder builder = new LCMB.ConcreteBuilder(materialOptions);
            LCMB.IBuilderDirector director = new LCMB.BuilderDirector(builder);
            return director.BuildMaterial();
        }

        private void SetMaterialOptions()
        {
            materialOptions.Strength = MainStrength;
            if (codeType == CodeTypes.EuroCode_2_1990)
            {
                materialOptions.CodesType = LCMB.CodesType.EC2_1990;
            }
            else if (codeType == CodeTypes.SP63_2018)
            {
                materialOptions.CodesType = LCMB.CodesType.SP63_2018;
            }
            else { throw new StructureHelperException($"{ErrorStrings.ObjectTypeIsUnknown} : {codeType}"); }
            if (limitState == LimitStates.ULS) { materialOptions.LimitState = LCMB.LimitStates.Collapse; }
            else if (limitState == LimitStates.SLS) { materialOptions.LimitState = LCMB.LimitStates.ServiceAbility; }
            else if (limitState == LimitStates.Special) { materialOptions.LimitState = LCMB.LimitStates.Special; }
            else { throw new StructureHelperException(ErrorStrings.LimitStatesIsNotValid); }
            if (calcTerm == CalcTerms.ShortTerm) { materialOptions.IsShortTerm = true; }
            else if (calcTerm == CalcTerms.LongTerm) { materialOptions.IsShortTerm = false; }
            else { throw new StructureHelperException(ErrorStrings.LoadTermIsNotValid); }
        }

        public (double Compressive, double Tensile) GetStrength(LimitStates limitState, CalcTerms calcTerm)
        {
            throw new NotImplementedException();
        }

        public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            throw new NotImplementedException();
        }

        public IHelperMaterial GetClone(IHelperMaterial sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }

        public IHelperMaterial GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            return new LibMaterial(id, MaterialType, codeType, Name, MainStrength);
        }
    }
}
