using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
using StructureHelperCommon.Services.ColorServices;
using System;
using System.Windows.Media;

namespace StructureHelperCommon.Models.VisualProperties
{
    /// <inheritdoc/>
    public class PrimitiveVisualProperty : IPrimitiveVisualProperty
    {
        private double opacity = 1;

        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public bool IsVisible { get; set; } = true;
        /// <inheritdoc/>
        public Color Color { get; set; } = ColorProcessor.GetRandomColor();
        /// <inheritdoc/>
        public int ZIndex { get; set; } = 0;
        /// <inheritdoc/>
        public double Opacity
        {
            get => opacity;
            set
            {
                if (value < 0)
                {
                    opacity = 0;
                    return;
                }
                if (value > 1)
                {
                    opacity = 1;
                    return;
                }
                opacity = value;
            }
        }


        public PrimitiveVisualProperty(Guid id)
        {
            Id = id;
        }

        public IPrimitiveVisualProperty GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            PrimitiveVisualProperty newItem = new(id);
            var logic = new PrimitiveVisualPropertyUpdateStrategy();
            logic.Update(newItem, this);
            return newItem;
        }
    }
}
