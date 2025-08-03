using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.VisualProperties;
using System.Windows.Media;


namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByDensity : IStirrupByDensity
    {
        private IUpdateStrategy<IStirrupByDensity> updateStrategy;
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public double StirrupDensity { get; set; }
        public double CompressedGap { get; set; }
        public IPrimitiveVisualProperty VisualProperty { get; set; }
        public double StartCoordinate { get; set; } = 0;
        public double EndCoordinate { get; set; } = 100;

        public StirrupByDensity(Guid id)
        {
            Id = id;
            VisualProperty = new PrimitiveVisualProperty(Guid.NewGuid())
            {
                Color = (Color)ColorConverter.ConvertFromString("Black")
            };
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
