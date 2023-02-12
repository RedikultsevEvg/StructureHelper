using System.Collections.Generic;
using System.Linq;
using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class MaterialSafetyFactor : IMaterialSafetyFactor
    {
        public string Name {get; set; }
        public bool Take { get; set; }
        public string Description { get; set; }
        public List<IMaterialPartialFactor> PartialFactors { get; }

        public MaterialSafetyFactor()
        {
            Take = true;
            Name = "New factor";
            Description = "Material safety factor for ...";
            PartialFactors = new List<IMaterialPartialFactor>();
        }

        public double GetFactor(StressStates stressState, CalcTerms calcTerm, LimitStates limitStates)
        {
            double result = 1d;
            var coefficients = PartialFactors.Where(x => (x.StressState == stressState & x.CalcTerm == calcTerm & x.LimitState == limitStates));
            foreach (var item in coefficients) { result *= item.FactorValue;}
            return result;
        }

        public object Clone()
        {
            var newItem = new MaterialSafetyFactor();
            newItem.Take = Take;
            newItem.Name = Name;
            newItem.Description = Description;
            foreach (var item in PartialFactors)
            {
                newItem.PartialFactors.Add(item.Clone() as IMaterialPartialFactor);
            }
            return newItem;
        }
    }
}
