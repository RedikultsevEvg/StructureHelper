using Newtonsoft.Json;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class CrossSectionRepositoryDTO : ICrossSectionRepository
    {
        private IRepositoryOperationsLogic operations;

        [JsonProperty("Id")]        
        public Guid Id { get; set; }
        [JsonProperty("HeadMaterials")]        
        public List<IHeadMaterial> HeadMaterials { get; } = [];
        [JsonProperty("ForceActions")]        
        public List<IForceAction> ForceActions { get; } = [];
        [JsonProperty("Primitives")]        
        public List<INdmPrimitive> Primitives { get; } = [];
        [JsonProperty("Calculators")]        
        public List<ICalculator> Calculators { get; } = [];
        [JsonIgnore]        
        public ICrossSection CrossSection { get; set; }

        public CrossSectionRepositoryDTO(Guid id)
        {
            Id = id;
        }

        [JsonIgnore]        
        public IRepositoryOperationsLogic Operations => operations ??= new RepositoryOperationsLogic(this);


        public ICrossSectionRepository GetClone(ICrossSectionRepository sourceObject, CloneMode cloningMode)
        {
            if (cloningMode == CloneMode.Reference)
            {
                return sourceObject;
            }

            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            var clone = new CrossSectionRepositoryDTO(id);
            var cloningStrategy = new DeepCloningStrategy();

            foreach (var material in sourceObject.HeadMaterials)
            {
                clone.HeadMaterials.Add(material.GetClone(cloningMode));
            }
            foreach (var action in sourceObject.ForceActions)
            {
                clone.ForceActions.Add(cloningStrategy.Clone(action, null, cloningMode));
            }
            foreach (var primitive in sourceObject.Primitives)
            {
                clone.Primitives.Add(primitive.GetClone(cloningMode));
            }
            foreach (var calculator in sourceObject.Calculators)
            {
                clone.Calculators.Add(cloningStrategy.Clone(calculator, null, cloningMode));
            }

            return clone;
        }

        public void AddHeadMaterial(IHeadMaterial item)
        {
            StructureHelperLogics.Models.CrossSections.CrossSectionRepositoryMutationLogic.AddHeadMaterial(this, item);
        }

        public bool RemoveHeadMaterial(IHeadMaterial item)
        {
            return StructureHelperLogics.Models.CrossSections.CrossSectionRepositoryMutationLogic.RemoveHeadMaterial(this, item);
        }

        public bool RemoveHeadMaterial(Guid id)
        {
            return StructureHelperLogics.Models.CrossSections.CrossSectionRepositoryMutationLogic.RemoveHeadMaterial(this, id);
        }

        public void AddForceAction(IForceAction item)
        {
            StructureHelperLogics.Models.CrossSections.CrossSectionRepositoryMutationLogic.AddForceAction(this, item);
        }

        public bool RemoveForceAction(IForceAction item)
        {
            return StructureHelperLogics.Models.CrossSections.CrossSectionRepositoryMutationLogic.RemoveForceAction(this, item);
        }

        public bool RemoveForceAction(Guid id)
        {
            return StructureHelperLogics.Models.CrossSections.CrossSectionRepositoryMutationLogic.RemoveForceAction(this, id);
        }

        public void AddPrimitive(INdmPrimitive item)
        {
            StructureHelperLogics.Models.CrossSections.CrossSectionRepositoryMutationLogic.AddPrimitive(this, item);
        }

        public bool RemovePrimitive(INdmPrimitive item)
        {
            return StructureHelperLogics.Models.CrossSections.CrossSectionRepositoryMutationLogic.RemovePrimitive(this, item);
        }

        public bool RemovePrimitive(Guid id)
        {
            return StructureHelperLogics.Models.CrossSections.CrossSectionRepositoryMutationLogic.RemovePrimitive(this, id);
        }

        public void AddCalculator(ICalculator item)
        {
            StructureHelperLogics.Models.CrossSections.CrossSectionRepositoryMutationLogic.AddCalculator(this, item);
        }

        public bool RemoveCalculator(ICalculator item)
        {
            return StructureHelperLogics.Models.CrossSections.CrossSectionRepositoryMutationLogic.RemoveCalculator(this, item);
        }

        public bool RemoveCalculator(Guid id)
        {
            return StructureHelperLogics.Models.CrossSections.CrossSectionRepositoryMutationLogic.RemoveCalculator(this, id);
        }

        public void ClearAll()
        {
            StructureHelperLogics.Models.CrossSections.CrossSectionRepositoryMutationLogic.ClearAll(this);
        }

        public ICrossSectionRepository GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            throw new NotImplementedException();
        }
    }
}
