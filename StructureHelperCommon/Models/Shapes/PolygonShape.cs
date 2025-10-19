using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Shapes
{
    public class PolygonShape : IPolygonShape
    {
        public Guid Id { get; }
        public List<IPolygonSegment> Segments { get; } = [];
        public PolygonShape(Guid id)
        {
            Id = id;
        }

    }
}
