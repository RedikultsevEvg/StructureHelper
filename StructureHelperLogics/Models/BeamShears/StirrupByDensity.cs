using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByDensity : IStirrupByDensity
    {
        private IUpdateStrategy<IStirrupByDensity> updateStrategy;
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public double StirrupDensity { get; set; }
        public double CompressedGap { get; set; }

        public StirrupByDensity(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            StirrupByDensity newItem = new(Guid.NewGuid());
            updateStrategy ??= new StirrupByDensityUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
