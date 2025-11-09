using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public interface IValueDiagram : ISaveable, ICloneable
    {
        IPoint2DRange Point2DRange { get; }
        public int StepNumber { get; set; }
    }
}
