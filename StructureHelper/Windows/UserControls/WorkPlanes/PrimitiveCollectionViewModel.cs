using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using System.Collections.ObjectModel;

namespace StructureHelper.Windows.UserControls.WorkPlanes
{
    public class PrimitiveCollectionViewModel : ViewModelBase
    {
        public ObservableCollection<IGraphicalPrimitive> Primitives { get; } = new();
    }
}
