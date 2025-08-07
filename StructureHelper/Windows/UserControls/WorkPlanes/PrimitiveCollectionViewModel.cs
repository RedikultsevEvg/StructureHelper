using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelperLogics.Models.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.UserControls.WorkPlanes
{
    public class PrimitiveCollectionViewModel : ViewModelBase
    {
        public ObservableCollection<IGraphicalPrimitive> Primitives { get; } = new();
    }
}
