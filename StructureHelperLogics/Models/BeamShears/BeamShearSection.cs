using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearSection : IBeamShearSection
    {
        public Guid Id { get; }
        public string? Name { get; set; }
        /// <inheritdoc/>
        public IMaterialStrength MaterialStrength { get; } = new MaterialStrengthByLibMaterial(Guid.NewGuid(), new ConcreteLibMaterial(Guid.NewGuid()));
        /// <inheritdoc/>
        public IShape Shape { get; } = new RectangleShape(Guid.NewGuid()) { Height = 0.6, Width = 0.4};

        public double CenterCover { get; set; }


        public BeamShearSection(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
