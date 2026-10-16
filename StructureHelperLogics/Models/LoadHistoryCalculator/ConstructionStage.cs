using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.LoadHistoryCalculator
{
    internal class ConstructionStage : IConstructionStage
    {
        private const int minSubSepNumber = 3;
        private int subStepNumber = 10;

        public Guid Id { get; }
        public string Name { get; set; } = "New construction stage";
        public int SubStepNumber
        {
            get => subStepNumber;
            set
            {
                subStepNumber = Math.Max(value, minSubSepNumber);
            }
        }
        public IForceTuple ForceTuple { get; set; } = new ForceTuple();
        public List<INdmPrimitive> Primitives { get; } = [];

        public ConstructionStage(Guid id)
        {
            Id = id;
        }
    }
}
