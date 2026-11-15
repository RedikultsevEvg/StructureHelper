using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperCommon.Services.ColorServices;
using System.Windows.Media;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class VisualProperty : IVisualProperty
    {
        
        public Guid Id { get; }



        public bool IsVisible { get; set; } = true;
        public Color Color { get; set; } = ColorProcessor.GetRandomColor();
        public bool SetMaterialColor { get; set; } = true;
        public int ZIndex { get; set; } = 0;
        private double opacity = 1d;

        public double Opacity
        {
            get { return opacity; }
            set
            {
                if (value < 0d || value > 1d) { throw new StructureHelperException(ErrorStrings.VisualPropertyIsNotRight + nameof(Opacity) + value); } 
                opacity = value;
            }
        }

        public VisualProperty(Guid id)
        {
            Id = id;
        }

        public VisualProperty() : this (Guid.NewGuid())
        {
            
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public IPrimitiveVisualProperty GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            throw new NotImplementedException();
        }
    }
}
