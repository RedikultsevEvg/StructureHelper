using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.DiagramTemplates;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Models.Materials.Logics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperCommon.Models.Materials
{
    public class CustomConcreteHelperMaterial : ICustomConcreteHelperMaterial
    {
        public Guid Id {  get; }
        public List<ICustomConcreteModel> MaterialModels { get; } = [];
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; } = [];
        public bool TensionForULS { get; set; } = false;
        public bool TensionForSLS { get; set; } = true;

        public CustomConcreteHelperMaterial(Guid id)
        {
            Id = id;
            SafetyFactors.AddRange(PartialFactorsFactory.GetDefaultConcreteSafetyFactors(ProgramSetting.CodeType));
        }


        public object Clone()
        {
            var clone = new CustomConcreteHelperMaterial(Guid.NewGuid());
            var logic = new CustomConcreteHelperMaterialUpdateStrategy() { UpdateChildren = true };
            logic.Update(clone, this);
            return clone;
        }

        public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            const bool removeWorkInTension = true;
            ICrackMaterial material = GetLoaderMaterial(limitState, calcTerm, removeWorkInTension);
            return material;
        }

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            bool workInTension;
            if (limitState == LimitStates.ULS) { workInTension = TensionForULS; }
            else if (limitState == LimitStates.SLS) { workInTension = TensionForSLS; }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(limitState));
            }
            ICrackMaterial material = GetLoaderMaterial(limitState, calcTerm, workInTension);
            return material;
        }

        private ICrackMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm, bool workInTension)
        {
            ICustomConcreteModel model = GetConcreteModel(limitState, calcTerm);
            IMaterialFactorLogic factorLogic = new MaterialFactorLogic(SafetyFactors);
            var factors = factorLogic.GetTotalFactor(LimitStates.ULS, CalcTerms.ShortTerm);
            var convertLogic = new CustomConcreteOptionsToLoaderConvertStrategy()
            {
                WorkInTension = workInTension,
                CompressionStrengthFactor = factors.Compressive,
                TensionStrengthFactor = factors.Tensile
            };
            var loaderOptions = convertLogic.Convert(model);
            var factory = new CustomConcreteMaterialFactory();
            var material = factory.GetConcreteMaterial(loaderOptions);
            return material;
        }

        private ICustomConcreteModel GetConcreteModel(LimitStates limitState, CalcTerms calcTerm)
        {
            List<ICustomConcreteModel> models = MaterialModels.Where(x =>
            x.StateCalcTermPair.LimitState == limitState
            && x.StateCalcTermPair.CalcTerm == calcTerm
            && x.IsActive == true)
                .ToList();
            if (models.Count() != 1)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": material must have single correct model, but was {models.Count} available models");
            }
            ICustomConcreteModel model = models.First();
            return model;
        }
    }
}
