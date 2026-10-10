using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.VisualProperties;
using System.Windows.Media;


namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByDensity : IStirrupByDensity
    {
        private IUpdateStrategy<IStirrupByDensity> updateStrategy;
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public string Name { get; set; } = string.Empty;
        /// <inheritdoc/>
        public double StirrupDensity { get; set; }
        /// <inheritdoc/>
        public double CompressedGap { get; set; }
        /// <inheritdoc/>
        public IPrimitiveVisualProperty VisualProperty { get; set; }
        /// <inheritdoc/>
        public double StartCoordinate { get; set; } = 0d;
        /// <inheritdoc/>
        public double EndCoordinate { get; set; } = 100d;
        /// <inheritdoc/>
        public bool CheckMinimumIntensity { get; set; } = true;

        public StirrupByDensity(Guid id)
        {
            Id = id;
            VisualProperty = new PrimitiveVisualProperty(Guid.NewGuid())
            {
                Color = (Color)ColorConverter.ConvertFromString("Gray"),
                Opacity = 0.5
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
