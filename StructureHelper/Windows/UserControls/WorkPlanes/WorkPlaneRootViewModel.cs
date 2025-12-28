using StructureHelper.Infrastructure;
using StructureHelper.Windows.Graphs;

namespace StructureHelper.Windows.UserControls.WorkPlanes
{
    public class WorkPlaneRootViewModel : ViewModelBase
    {
        public bool IsToolBarVisible { get; set; } = true;
        public bool IsStatusBarVisible { get; set; } = true;
        public WorkPlaneConfigViewModel WorkPlaneConfig { get; } = new();
        public PrimitiveCollectionViewModel PrimitiveCollection { get; } = new();
    }
}
