using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Shapes
{
    public interface IPolygonSegment : ISaveable
    {
        IVertex StartVertex { get; }
        IVertex EndVertex { get; }
        void UpdateEndFromParameters();
    }
}
