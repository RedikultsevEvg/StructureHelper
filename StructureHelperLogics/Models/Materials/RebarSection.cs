using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    /// <inheritdoc/>
    public class RebarSection : IRebarSection
    {
        private const double minRebarDiameter = 0.001;
        private const double maxRebarDiameter = 0.050;
        private double diameter;

        /// <inheritdoc/>
        public Guid Id { get;}
        /// <inheritdoc/>
        public IReinforcementLibMaterial Material { get; set; }
        /// <inheritdoc/>
        public double Diameter
        {
            get => diameter; set
            {
                if (value < minRebarDiameter)
                {
                    throw new StructureHelperException($"Diameter of stirrup must not be less than {minRebarDiameter * 1000}mm, but was {value}");
                }
                if (value > maxRebarDiameter)
                {
                    throw new StructureHelperException($"Diameter of stirrup must be less or equal than {maxRebarDiameter * 1000}mm, but was {value}");
                }
                diameter = value;
            }
        }
        public RebarSection(Guid id)
        {
            Id = id;
            Diameter = 0.008;
            Material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Reinforcement400).HelperMaterial as IReinforcementLibMaterial;
        }

        public object Clone()
        {
            var newItem = new RebarSection(Guid.NewGuid());
            var logic = new RebarSectionUpdateStrategy();
            logic.Update(newItem, this);
            return newItem;
        }
    }
}
