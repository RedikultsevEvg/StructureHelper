using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services.ColorServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
