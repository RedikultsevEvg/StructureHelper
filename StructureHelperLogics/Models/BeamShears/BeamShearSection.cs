using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperLogics.Models.Materials;
using System.Windows.Media;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearSection : IBeamShearSection
    {
        private IUpdateStrategy<IBeamShearSection> updateStrategy;
        public Guid Id { get; }
        public string? Name { get; set; }
        /// <inheritdoc/>
        public IConcreteLibMaterial ConcreteMaterial { get; set; } 
        /// <inheritdoc/>
        public IShape Shape { get; set; } = new RectangleShape(Guid.NewGuid()) { Height = 0.6, Width = 0.4};

        public double CenterCover { get; set; } = 0.05;
        public double ReinforcementArea { get; set; } = 0;
        public IReinforcementLibMaterial ReinforcementMaterial { get; set; }
        public IPrimitiveVisualProperty VisualProperty { get; set; }

        public BeamShearSection(Guid id)
        {
            Id = id;
            ConcreteMaterial = ConcreteLibMaterialFactory.GetConcreteLibMaterial(ConcreteLibTypes.Concrete25);
            ReinforcementMaterial = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Reinforcement500).HelperMaterial as IReinforcementLibMaterial;
            ConcreteMaterial.TensionForULS = true;
            VisualProperty = new PrimitiveVisualProperty(Guid.NewGuid())
            {
                Color = (Color)ColorConverter.ConvertFromString("DarkGray")
            };
        }

        public object Clone()
        {
            var cloneStrategy = new ShapeCloneStrategy();
            IShape shapeClone = cloneStrategy.GetClone(Shape);
            BeamShearSection newItem = new(Guid.NewGuid()) { Shape = shapeClone};
            updateStrategy ??= new BeamShearSectionUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
