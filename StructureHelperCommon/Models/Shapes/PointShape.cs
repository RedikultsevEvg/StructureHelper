using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class PointShape : IPointShape
    {
        public Guid Id { get; }
        public double Area { get; set; }

        public PointShape(Guid id)
        {
            Id = id;
        }

        public PointShape() : this (Guid.NewGuid())
        {
            
        }
    }
}
