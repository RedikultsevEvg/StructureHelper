using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperLogics.Models.Materials;
using System.Windows.Media;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class StirrupByRebar : IStirrupByRebar
    {
        private IUpdateStrategy<IStirrupByRebar> updateStrategy;
        private double diameter = 0.008;
        private double step = 0.1;
        private double legCount = 2;

        /// <inheritdoc/>
        public Guid Id { get; }

        public string? Name { get; set; }
        /// <inheritdoc/>
        public IReinforcementLibMaterial Material { get; set; }
        /// <inheritdoc/>
        public double LegCount
        {
            get => legCount; set
            {
                if (value < 0)
                {
                    throw new StructureHelperException("Number of legs of stirrup must be greater than zero");
                }
                legCount = value;
            }
        }         
        /// <inheritdoc/>
        public double Diameter
        {
            get => diameter; set
            {
                if (value < 0.001)
                {
                    throw new StructureHelperException("Diameter of stirrup must not be less than 1mm");
                }
                if (value > 0.050)
                {
                    throw new StructureHelperException("Diameter of stirrup must be less or equal than 50mm");
                }
                diameter = value;
            }
        }         
        /// <inheritdoc/>
        public double Spacing
        {
            get => step; set
            {
                if (value < 0.01)
                {
                    throw new StructureHelperException("Spacing of stirrups must not be less than 10mm");
                }
                step = value;
            }
        }
        /// <inheritdoc/>
        public double CompressedGap { get; set; } = 0;
        /// <inheritdoc/>
        public bool IsSpiral { get; set; } = false;
        public double StartCoordinate { get; set; } = 0;
        public double EndCoordinate { get; set; } = 100;
        public IPrimitiveVisualProperty VisualProperty { get; set; }

        public StirrupByRebar(Guid id)
        {
            Id = id;
            Material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Reinforcement400).HelperMaterial as IReinforcementLibMaterial;
            VisualProperty = new PrimitiveVisualProperty(Guid.NewGuid())
            {
                Color = (Color)ColorConverter.ConvertFromString("Brown")
            };
        }

        public object Clone()
        {
            StirrupByRebar stirrupByRebar = new(Guid.NewGuid());
            updateStrategy ??= new StirrupByRebarUpdateStrategy();
            updateStrategy.Update(stirrupByRebar, this); 
            return stirrupByRebar;
        }
    }
}
