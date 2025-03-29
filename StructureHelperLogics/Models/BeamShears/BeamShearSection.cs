using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearSection : IBeamShearSection
    {
        private IUpdateStrategy<IBeamShearSection> updateStrategy;
        public Guid Id { get; }
        public string? Name { get; set; }
        /// <inheritdoc/>
        public IConcreteLibMaterial Material { get; set; } 
        /// <inheritdoc/>
        public IShape Shape { get; } = new RectangleShape(Guid.NewGuid()) { Height = 0.6, Width = 0.4};

        public double CenterCover { get; set; } = 0.05;


        public BeamShearSection(Guid id)
        {
            Id = id;
            Material = ConcreteLibMaterialFactory.GetConcreteLibMaterial(ConcreteLibTypes.Concrete25);
            Material.TensionForULS = true;
        }

        public object Clone()
        {
            BeamShearSection newItem = new(Guid.NewGuid());
            updateStrategy ??= new BeamShearSectionUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
