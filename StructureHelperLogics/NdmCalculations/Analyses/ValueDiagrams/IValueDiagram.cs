using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public interface IValueDiagram : ISaveable, ISelfCloneStrategy<IValueDiagram>
    {
        IPoint2DRange Point2DRange { get; set; }
        public int StepNumber { get; set; }
    }
}
