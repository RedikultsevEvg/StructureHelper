using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class PolygonLineSegment : IPolygonLineSegment
    {
        public Guid Id { get; }
        public IVertex StartVertex { get; }
        public IVertex EndVertex { get; }


        public PolygonLineSegment(Guid id)
        {
            Id = id;
        }

        public void UpdateEndFromParameters()
        {
            //nothing to do
        }
    }
}
