using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class MaterialSafetyFactor : IMaterialSafetyFactor
    {
        public Guid Id { get; }
        public string Name {get; set; }
        public bool Take { get; set; }
        public string Description { get; set; }
        public List<IMaterialPartialFactor> PartialFactors { get; }


        public MaterialSafetyFactor(Guid id)
        {
            Id = id;
            Take = true;
            Name = "New factor";
            Description = "Material safety factor for ...";
            PartialFactors = new List<IMaterialPartialFactor>();
        }

        public MaterialSafetyFactor() : this (Guid.NewGuid())
        {         }

        public double GetFactor(StressStates stressState, CalcTerms calcTerm, LimitStates limitStates)
        {
            double result = 1d;
            var coefficients = PartialFactors
                .Where(x => x.StressState == stressState
                & x.CalcTerm == calcTerm
                & x.LimitState == limitStates);
            foreach (var item in coefficients) { result *= item.FactorValue;}
            return result;
        }

        public IMaterialSafetyFactor GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            var newItem = new MaterialSafetyFactor(id);
            var updateSrategy = new MaterialSafetyFactorUpdateStrategy() { UpdateChildren = false };
            updateSrategy.Update(newItem, this);
            newItem.PartialFactors.Clear();
            foreach (var item in this.PartialFactors)
            {
                newItem.PartialFactors.Add((IMaterialPartialFactor)item.GetClone(cloneMode));
            }
            return newItem;
        }
    }
}
