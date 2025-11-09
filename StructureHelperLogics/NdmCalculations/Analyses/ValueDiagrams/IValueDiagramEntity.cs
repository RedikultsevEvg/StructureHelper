using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public interface IValueDiagramEntity : ISaveable, ICloneable
    {
        string Name { get; set; }
        bool IsTaken { get; set; }
        IValueDiagram ValueDigram { get; set; }
    }
}
