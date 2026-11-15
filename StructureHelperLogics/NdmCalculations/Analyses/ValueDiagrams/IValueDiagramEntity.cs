using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public interface IValueDiagramEntity : ISaveable, ISelfCloneStrategy<IValueDiagramEntity>
    {
        string Name { get; set; }
        bool IsTaken { get; set; }
        IValueDiagram ValueDiagram { get; set; }
    }
}
