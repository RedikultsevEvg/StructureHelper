using StructureHelper.Infrastructure;
using StructureHelper.Windows.Graphs;

namespace StructureHelper.Windows.UserControls.WorkPlanes
{
    public class WorkPlaneRootViewModel : ViewModelBase
    {
        public WorkPlaneConfigViewModel WorkPlaneConfig { get; } = new();
        public PrimitiveCollectionViewModel PrimitiveCollection { get; } = new();
        //public SaveCopyFWElementViewModel SaveCopyViewModel { get; } = new();
    }
}
