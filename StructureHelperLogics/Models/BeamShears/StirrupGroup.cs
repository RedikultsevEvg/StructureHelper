using StructureHelperCommon.Models.VisualProperties;
using System.Windows.Media;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupGroup : IStirrupGroup
    {
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public List<IStirrup> Stirrups { get; } = new();
        public double CompressedGap { get; set; }
        public IPrimitiveVisualProperty VisualProperty { get; set; }

        public StirrupGroup(Guid id)
        {
            Id = id;
            VisualProperty = new PrimitiveVisualProperty(Guid.NewGuid())
            {
                Color = (Color)ColorConverter.ConvertFromString("Black")
            };
        }

        public object Clone()
        {
            var updateStrategy = new StirrupGroupUpdateStrategy();
            StirrupGroup newItem = new(Guid.NewGuid());
            updateStrategy.Update(newItem, this);
            return newItem;
        }

    }
}
